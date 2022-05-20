using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public class InvoiceService
    {

        #region Descripción de la clase Invoice Service
        /*
         * Aqui esta definida la logica de la entidad invoice con los metodos publicos y encapsulados
         * Los metodos presentes en esta clase hace la funcion de un CRUD
         * INSERT
         * UPDATE
         * DELETE
         * GETALL
         * GETID
         * **/
        #endregion

        #region Metodos Publicos
        //Obtenemos la lista de todas las invoice
        public List<Invoice> GetAll()
        {
            var result =  new List<Invoice>();

            using (var context = new SqlConnection(Parameters.ConnectionString))
            {
                context.Open();
                var command = new SqlCommand("Select * from invoices", context);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var invoice = new Invoice
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Iva = Convert.ToDecimal(reader["iva"]),
                            SubTotal = Convert.ToDecimal(reader["subtotal"]),
                            Total = Convert.ToDecimal(reader["total"]),
                            ClientId = Convert.ToInt32(reader["clientId"])
                        };
                        result.Add(invoice);
                    }
                }

                //Set aditinal properties
                foreach (var invoice in result)
                {
                    //Client
                    SetClient(invoice, context);
                    //Detail
                    SetDetail(invoice, context);
                }
            }
            return result;
        }
        //Obtenemos un invoice por id
        public Invoice Get(int id)
        {
            var result = new Invoice();
            using (var context  = new SqlConnection(Parameters.ConnectionString))
            {
                context.Open();
                var command = new SqlCommand("Select * from invoices where id = @invoiceId",context);
                command.Parameters.AddWithValue("@invoiceId",id);
                using (var reader = command.ExecuteReader()) {
                    reader.Read();
                    result = new Invoice
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        ClientId = Convert.ToInt32(reader["clientId"]),
                        Iva = Convert.ToDecimal(reader["iva"]),
                        Total = Convert.ToDecimal(reader["total"]),
                        SubTotal = Convert.ToDecimal(reader["subTotal"])
                        
                    };
     
                }

                SetClient(result, context);
                SetDetail(result, context);
               
            }
            return result;
        }
        //Creamos un invoice con su detalle 
        public void Create ( Invoice model)
        {
            PrepareOrder(model);
            using (var tranx = new TransactionScope())
            {
                using (var context = new SqlConnection(Parameters.ConnectionString))
                {
                    context.Open();
                    //Header
                    AddHeader(model, context);
                    //Details
                    AddDetail(model, context);
                }
                tranx.Complete();
            }
          
        }
        //Actualizamos un invoice con su detalle
        public void Update(Invoice model)
        {
            PrepareOrder(model);
            using (var tranx = new TransactionScope())
            {
                using (var context = new SqlConnection(Parameters.ConnectionString))
                {
                    context.Open();
                    //Header
                    UpdateHeader(model, context);
                    //Remove 
                    RemoveDetail(model.Id, context);
                    //Details
                    AddDetail(model, context);
                }
                tranx.Complete();
            }
                
        }
        //Elminar un invoice con su detalle
        public void Delete(int id)
        {
            using (var context  = new SqlConnection(Parameters.ConnectionString))
            {
                //Abrimos la conexion
                context.Open();
                //Primero eliminar el detalle de la factura
                RemoveDetail(id, context);
                //Segundo eliminar la factura luego de que el detalla este elimnado 
                DeleteHeader(id, context);
            }
        }


        #endregion

        #region Metodos Encapsulados
        //Metodo encapsulado para crear la cabecera del metodo create
        private void AddHeader(Invoice model, SqlConnection context)
        {
            var query = "insert into invoices(clientId,Iva,SubTotal,Total) output INSERTED.ID values(@clientId,@iva, @subTotal,@total)";
            var command = new SqlCommand(query, context);
            command.Parameters.AddWithValue("@iva", model.Iva);
            command.Parameters.AddWithValue("@clientId", model.ClientId);
            command.Parameters.AddWithValue("@subTotal", model.SubTotal);
            command.Parameters.AddWithValue("@total", model.Total);

            model.Id = Convert.ToInt32(command.ExecuteScalar());

        }
        //Metodo encapsulado para modificar la cabecera del metodo update
        private void UpdateHeader(Invoice model, SqlConnection context)
        {
            var query = "update invoices set clientId = @clientId,iva = @iva, subTotal = @subTotal,total = @total WHERE id = @id ";
            var command = new SqlCommand(query, context);
            command.Parameters.AddWithValue("@iva", model.Iva);
            command.Parameters.AddWithValue("@clientId", model.ClientId);
            command.Parameters.AddWithValue("@subTotal", model.SubTotal);
            command.Parameters.AddWithValue("@total", model.Total);
            command.Parameters.AddWithValue("@id", model.Id);
            command.ExecuteNonQuery();

        }
        //Metodo encapsulado para eliminar la cabecera del metodo delete
        private void DeleteHeader(int invoiceId,SqlConnection context)
        {
            var query = "delete from invoices where id = @id";
            var command = new SqlCommand(query,context);
            command.Parameters.AddWithValue("@id",invoiceId);
            command.ExecuteNonQuery();
        }
        //Insertamos el detalle a la cabecera
        private void AddDetail(Invoice invoice, SqlConnection context)
        {
            foreach (var detail in invoice.Detail)
            {

                var query = "insert into invoiceDetail(productId,InvoiceId,quantity,price,iva,subTotal,total) values (@productId,@invoiceId,@quantity,@price,@iva,@subtotal,@total)";
                var command = new SqlCommand(query, context);
                command.Parameters.AddWithValue("@iva", detail.Iva);
                command.Parameters.AddWithValue("@price", detail.Price);
                command.Parameters.AddWithValue("@total", detail.Total);
                command.Parameters.AddWithValue("@subtotal", detail.SubTotal);
                command.Parameters.AddWithValue("@quantity", detail.Quantity);
                command.Parameters.AddWithValue("@invoiceId", invoice.Id);
                command.Parameters.AddWithValue("@productId", detail.ProductoId);

                command.ExecuteNonQuery();
            }
        }
        //Eliminar el detalle anterior
        private void RemoveDetail(int invoiceId, SqlConnection context)
        {

            var query = "delete from invoicedetail WHERE invoiceId = @invoiceId";
            var command = new SqlCommand(query, context);
            command.Parameters.AddWithValue("@invoiceId", invoiceId);
            command.ExecuteNonQuery();

        }
        //Premaparamos los Calculos de subtotal,iva,total antes de insertalo con este metodos
        private void PrepareOrder(Invoice model)
        {
            foreach (var detail in model.Detail)
            {
                detail.Total = detail.Quantity * detail.Price;
                detail.Iva = detail.Total * Parameters.IvaRate;
                detail.SubTotal = detail.Total - detail.Iva;
            }
            model.Total = model.Detail.Sum(x => x.Total);
            model.SubTotal = model.Detail.Sum(x => x.SubTotal);
            model.Iva = model.Detail.Sum(x => x.Iva);
        }
        //Insertamor la cabecera

        //Obtenemos el cliente y luego lo agregamos al invoice correspondiente
        private void SetClient(Invoice invoice, SqlConnection context)
        {
            var command = new SqlCommand("select * from clients where id =  @clientId", context);
            command.Parameters.AddWithValue("@clientId", invoice.ClientId);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                invoice.Client = new Client
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                };
            }

        }
        //Obtenemos todo los detalles del invoice hacemos el mapper y lo agregamos al invoice solicitado.
        private void SetDetail(Invoice invoice, SqlConnection context)
        {

            var command = new SqlCommand("select * from InvoiceDetail where invoiceId =  @invoiceId", context);
            command.Parameters.AddWithValue("@invoiceId", invoice.Id);

            using (var reader = command.ExecuteReader())
            {

                while (reader.Read())
                {
                    invoice.Detail.Add(new InvoiceDetail
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Iva = Convert.ToDecimal(reader["iva"]),
                        Price = Convert.ToDecimal(reader["price"]),
                        Quantity = Convert.ToInt32(reader["quantity"]),
                        SubTotal = Convert.ToDecimal(reader["subtotal"]),
                        Total = Convert.ToDecimal(reader["total"]),
                        Invoice = invoice,
                        ProductoId = Convert.ToInt32(reader["productId"]),
                    });
                }
            }
            //Set Product
            foreach (var detail in invoice.Detail)
            {
                //Product 
                SetProduct(detail, context);
            }

        }
        //Obtenemos todo los producto del detalle del invoice hacemos el mapper y lo agregamos al detalle solicitado.
        private void SetProduct(InvoiceDetail invoiceDetail, SqlConnection context)
        {
            var command = new SqlCommand("select * from products where id = @productid", context);
            command.Parameters.AddWithValue("@productid", invoiceDetail.ProductoId);

            using (var reader = command.ExecuteReader())
            {
                reader.Read();

                invoiceDetail.Product = new Product
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    Price = Convert.ToDecimal(reader["price"])
                };
            }
        }
        #endregion
    }
}
