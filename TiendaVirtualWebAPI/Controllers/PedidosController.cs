using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using TiendaVirtualWebAPI.Models;
using TiendaVirtualWebAPI.ViewModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TiendaVirtualWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly TiendaVirtual20221Context _context;

        public PedidosController(TiendaVirtual20221Context context)
        {
            _context = context;
        }
        // GET: api/<PedidosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PedidosController>/5
        [HttpGet("{id}")]
        public ActionResult<vmPedido> Get(int id)
        {
            Pedido bdPed = _context.Pedidos.FirstOrDefault(x => x.Id == id);

            if (bdPed != null)
            {
                vmPedido objPed = new vmPedido();
                //Llenar Cabecera
                objPed.Id = bdPed.Id;
                objPed.Estado = bdPed.Estado;
                objPed.FechaHora = bdPed.FechaHora;
                objPed.IdCliente = bdPed.IdCliente;
                objPed.IdTarjeta = bdPed.IdTarjeta;
                objPed.Total = bdPed.Total;

                //llenando datos cliente
                _context.Entry(bdPed).Reference("IdClienteNavigation").Load();

                vmCliente objCliente = new vmCliente()
                {
                    Apellidos = bdPed.IdClienteNavigation.Apellidos,
                    Correo = bdPed.IdClienteNavigation.Correo,
                    Direccion = bdPed.IdClienteNavigation.Direccion,
                    Dni = bdPed.IdClienteNavigation.Dni,
                    Id = bdPed.IdClienteNavigation.Id,
                    Nombres = bdPed.IdClienteNavigation.Nombres,
                    Telefono = bdPed.IdClienteNavigation.Telefono
                };

                objPed.Cliente = objCliente;

                //llenar datos de la tarjeta

                _context.Entry(bdPed).Reference("IdTarjetaNavigation").Load();

                vmTarjeta objTarjeta = new vmTarjeta()
                {
                    Marca = bdPed.IdTarjetaNavigation.Marca,
                    Numero = bdPed.IdTarjetaNavigation.Numero,
                    Id = bdPed.IdTarjetaNavigation.Id

                };

                objPed.Tarjeta = objTarjeta;


                //llenando datos del detalle del pedido (todos los datos de "vmPedidoDetalle")
                _context.Entry(bdPed).Collection("PedidoDetalles").Load();

                foreach (var item in bdPed.PedidoDetalles)
                {
                    vmPedidoDetalle objdet = new vmPedidoDetalle()
                    {
                        Cantidad = item.Cantidad,
                        Id = item.Id,
                        IdPedido = item.IdPedido,
                        IdProducto = item.IdProducto,
                        PrecioUnitario = item.PrecioUnitario,
                        SubTotal = item.SubTotal
                    };

                    //llenar datos del producto
                    _context.Entry(item).Reference("IdProductoNavigation").Load();

                    vmProducto objprod = new vmProducto()
                    {
                        Activo = item.IdProductoNavigation.Activo,
                        Descripcion = item.IdProductoNavigation.Descripcion,
                        Destacado = item.IdProductoNavigation.Destacado,
                        Id = item.IdProductoNavigation.Id,
                        IdCategoria = item.IdProductoNavigation.IdCategoria,
                        IdMarca = item.IdProductoNavigation.IdMarca,
                        Nombre = item.IdProductoNavigation.Nombre,
                        Precio = item.IdProductoNavigation.Precio,
                        Url = item.IdProductoNavigation.Url
                    };
                    objdet.Producto = objprod;

                    objPed.Detalles.Add(objdet);

                }

                return Ok(objPed);
            }

            return NotFound();
        }
        
        
        //  api/Pedidos/Pendiente?IdCliente=5
        [Route("Pendiente")]
        [HttpGet]
        public ActionResult<vmPedido> GetPendiente(int IdCliente)
        {
            Pedido bdPed = _context.Pedidos.FirstOrDefault(x => x.IdCliente == IdCliente && x.Estado == "Pendiente");

            if (bdPed != null)
            {
                vmPedido objPed = ConstruirObjetoPedido(bdPed);

                return Ok(objPed);

            }

            return NotFound();
        }



        /// <summary>
        /// POST api/<PedidosController>/NuevoPedido
        /// Creará un nuevo pedido solo si no hay pedidos pendientes, de lo contrario devolverá el pedido pendiente
        /// </summary>
        /// <param name="value"></param>
        [Route("NuevoPedido")]
        [HttpPost]
        public ActionResult<vmPedido> NuevoPedido([FromBody] vmPedido pPed)
        {
            Pedido bdPed = _context.Pedidos.FirstOrDefault(x => x.IdCliente == pPed.IdCliente && x.Estado == "Pendiente");
            vmPedido objPed;

            if (bdPed != null)
            {
                objPed = ConstruirObjetoPedido(bdPed);

            }
            else
            {
                bdPed = new Pedido()
                {
                    Estado = "Pendiente",
                    FechaHora = DateTime.Now,
                    IdCliente = pPed.IdCliente,
                    IdTarjeta = pPed.IdTarjeta,
                    Total = 0
                };

                _context.Pedidos.Add(bdPed);
                _context.SaveChanges();

                objPed = ConstruirObjetoPedido(bdPed);
            }

            return Ok(objPed);
        }

        /// <summary>
        /// POST api/<PedidosController>/NuevoDetalle
        /// Agregará un nuevo Item al detalle del pedido, sólo si el producto no existe en el detalle
        /// Por el contrario si el producto ya existe en el pedido se incrementará la cantidad
        /// </summary>
        /// <param name="pDet"></param>
        /// <returns></returns>
        [Route("NuevoDetalle")]
        [HttpPost]
        public ActionResult<vmPedidoDetalle> NuevoDetalle([FromBody] vmPedidoDetalle pDet)
        {
            PedidoDetalle bdDet;
            //Preguntar si producto ya existe en el pedido en este caso
            //incrementamos la cantidad del producto y recalculamos subtotal
            bdDet = _context.PedidoDetalles.FirstOrDefault(x => x.IdPedido == pDet.IdPedido && x.IdProducto == pDet.IdProducto);
            if (bdDet != null)
            {
                decimal? vcantidad = Convert.ToDecimal(bdDet.Cantidad) + 1;
                bdDet.Cantidad = (int)vcantidad;
                bdDet.SubTotal = Math.Round((decimal)(vcantidad * bdDet.PrecioUnitario), 2);

                _context.PedidoDetalles.Update(bdDet);
                _context.SaveChanges();

                ActualizarTotal((int)pDet.IdPedido);
            }
            else
            {   //Si producto no exsite buscamos los datos del producto para obtner el precio
                Producto bdProd = _context.Productos.FirstOrDefault(x => x.Id == pDet.IdProducto);
                if (bdProd != null)
                {      //obtener el precio del producto en caso el el precio enviado sea cero
                    decimal? punit = pDet.PrecioUnitario == 0 ? bdProd.Precio : pDet.PrecioUnitario;
                    bdDet = new PedidoDetalle()
                    {
                        Cantidad = pDet.Cantidad,
                        //Id = pDet.Id,
                        IdPedido = pDet.IdPedido,
                        IdProducto = pDet.IdProducto,
                        PrecioUnitario = punit,
                        SubTotal = Math.Round(Convert.ToDecimal(pDet.Cantidad) * (decimal)punit, 2)
                    };

                    _context.PedidoDetalles.Add(bdDet);
                    _context.SaveChanges();

                    ActualizarTotal((int)pDet.IdPedido);

                    pDet.Id = bdDet.Id;

                    return Ok(pDet);
                }
            }
            return NotFound();
        }


        // DELETE api/<PedidosController>/DeleteDetalle?IdDetalle=5
        [Route("DeleteDetalle")]
        [HttpDelete]
        public ActionResult<vmPedidoDetalle> DeleteDetalle(int IdDetalle)
        {
            PedidoDetalle bdDet = _context.PedidoDetalles.FirstOrDefault(x => x.Id == IdDetalle);
            if (bdDet != null)
            {
                vmPedidoDetalle objdet = new vmPedidoDetalle()
                {
                    Cantidad = bdDet.Cantidad,
                    Id = bdDet.Id,
                    IdPedido = bdDet.IdPedido,
                    IdProducto = bdDet.IdProducto,
                    PrecioUnitario = bdDet.PrecioUnitario,
                    SubTotal = bdDet.SubTotal
                };

                _context.PedidoDetalles.Remove(bdDet);
                _context.SaveChanges();

                ActualizarTotal((int)objdet.IdPedido);

                return Ok(objdet);
            }

            return NotFound();
        }



        /// <summary>
        /// Función general para construir un objeto pedido
        /// </summary>
        /// <param name="pbdPed"></param>
        /// <returns></returns>
        private vmPedido ConstruirObjetoPedido(Pedido pbdPed)
        {
            vmPedido objPed = new vmPedido();
            //Llenar Cabecera
            objPed.Id = pbdPed.Id;
            objPed.Estado = pbdPed.Estado;
            objPed.FechaHora = pbdPed.FechaHora;
            objPed.IdCliente = pbdPed.IdCliente;
            objPed.IdTarjeta = pbdPed.IdTarjeta;
            objPed.Total = pbdPed.Total;

            //llenando datos cliente
            _context.Entry(pbdPed).Reference("IdClienteNavigation").Load();

            vmCliente objCliente = new vmCliente()
            {
                Apellidos = pbdPed.IdClienteNavigation.Apellidos,
                Correo = pbdPed.IdClienteNavigation.Correo,
                Direccion = pbdPed.IdClienteNavigation.Direccion,
                Dni = pbdPed.IdClienteNavigation.Dni,
                Id = pbdPed.IdClienteNavigation.Id,
                Nombres = pbdPed.IdClienteNavigation.Nombres,
                Telefono = pbdPed.IdClienteNavigation.Telefono
            };

            objPed.Cliente = objCliente;

            //llenar datos de la tarjeta

            _context.Entry(pbdPed).Reference("IdTarjetaNavigation").Load();

            vmTarjeta objTarjeta = new vmTarjeta()
            {
                Marca = pbdPed.IdTarjetaNavigation.Marca,
                Numero = pbdPed.IdTarjetaNavigation.Numero,
                Id = pbdPed.IdTarjetaNavigation.Id

            };

            objPed.Tarjeta = objTarjeta;
            //llenando datos del detalle del pedido

            _context.Entry(pbdPed).Collection("PedidoDetalles").Load();

            foreach (var item in pbdPed.PedidoDetalles)
            {
                vmPedidoDetalle objdet = new vmPedidoDetalle()
                {
                    Cantidad = item.Cantidad,
                    Id = item.Id,
                    IdPedido = item.IdPedido,
                    IdProducto = item.IdProducto,
                    PrecioUnitario = item.PrecioUnitario,
                    SubTotal = item.SubTotal
                };

                //llenar datos del producto
                _context.Entry(item).Reference("IdProductoNavigation").Load();

                vmProducto objprod = new vmProducto()
                {
                    Activo = item.IdProductoNavigation.Activo,
                    Descripcion = item.IdProductoNavigation.Descripcion,
                    Destacado = item.IdProductoNavigation.Destacado,
                    Id = item.IdProductoNavigation.Id,
                    IdCategoria = item.IdProductoNavigation.IdCategoria,
                    IdMarca = item.IdProductoNavigation.IdMarca,
                    Nombre = item.IdProductoNavigation.Nombre,
                    Precio = item.IdProductoNavigation.Precio,
                    Url = item.IdProductoNavigation.Url
                };
                objdet.Producto = objprod;

                objPed.Detalles.Add(objdet);

            }

            return objPed;


        }


        private void ActualizarTotal(int IdPedido)
        {
            Pedido bdped = _context.Pedidos.FirstOrDefault(x => x.Id == IdPedido);

            if (bdped != null)
            {
                bdped.Total = _context.PedidoDetalles.Where(x => x.IdPedido == IdPedido).Sum(x => x.SubTotal);
                _context.Pedidos.Update(bdped);
                _context.SaveChanges();
            }
        }




        // PUT api/<PedidosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }


    }
}
