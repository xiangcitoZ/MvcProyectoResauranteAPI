﻿using Newtonsoft.Json;
using NuggetRestauranteXZX.Models;
using System.Net.Http.Headers;
using System.Text;

namespace MvcProyectoResauranteAPI.Services
{
    public class ServiceApiRestaurante
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        public ServiceApiRestaurante(IConfiguration configuration)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");
            this.UrlApi = configuration.GetValue<string>
                ("ApiUrls:ApiProyectoRestaurante");
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        //ITEM MENU

        public async Task<List<ItemMenu>> GetItemMenuAsync()
        {
            string request = "/api/ItemMenu";
            List<ItemMenu> itemMenus =
                await this.CallApiAsync<List<ItemMenu>>(request);
            return itemMenus;
        }

 
        public async Task DeleteItemMenuAsync(int idmenu)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/ItemMenu/" + idmenu;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response =
                    await client.DeleteAsync(request);

            }
        }

        public async Task InsertItemMenuAsync
            (int idmenu, string nombre, string categoria, string imagen, decimal precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/ItemMenu/CreateItemMenus";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                ItemMenu menu = new ItemMenu();
                menu.IdMenu = idmenu;
                menu.Nombre = nombre;   
                menu.Categoria = categoria;
                menu.Imagen = imagen;
                menu.Precio = precio;

                string json = JsonConvert.SerializeObject(menu);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);

            }
        }

        public async Task UpdateItemMenuAsync
            (int idmenu, string nombre, string categoria, string imagen, decimal precio)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/ItemMenu" + idmenu;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                ItemMenu pj =
                    new ItemMenu
                    {
                        IdMenu = idmenu,
                        Nombre = nombre,
                        Categoria = categoria,
                        Imagen = imagen,
                        Precio = precio
                    };

                string json = JsonConvert.SerializeObject(pj);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync(request, content);
            }
        }

        //MESA

        public async Task<List<Mesa>> GetMesaAsync()
        {
            string request = "/api/Mesa";
            List<Mesa> Mesa =
                await this.CallApiAsync<List<Mesa>>(request);
            return Mesa;
        }


        public async Task DeleteMesaAsync(int idmesa)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Mesa/" + idmesa;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response =
                    await client.DeleteAsync(request);

            }
        }

        public async Task InsertMesaAsync
            (int idmesa, string estado, int cantidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Mesa/CreateMesa";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Mesa mesa = new Mesa();
                mesa.IdMesa = idmesa;
                mesa.Estado = estado;
                mesa.Cantidad = cantidad;

                string json = JsonConvert.SerializeObject(mesa);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);

            }
        }

        public async Task UpdateMesaAsync
           (int idmesa, string estado, int cantidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Mesa/" + idmesa;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Mesa mesa =
                    new Mesa
                    {
                        IdMesa = idmesa,
                        Estado = estado,
                        Cantidad = cantidad
                    };

                string json = JsonConvert.SerializeObject(mesa);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync(request, content);
            }
        }


        //PEDIDO

        public async Task<List<Pedido>> GetPedidoAsync()
        {
            string request = "/api/Pedido";
            List<Pedido> Pedido =
                await this.CallApiAsync<List<Pedido>>(request);
            return Pedido;
        }


        public async Task DeletePedidoAsync(int idpedido)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Pedido/" + idpedido;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();

                HttpResponseMessage response =
                    await client.DeleteAsync(request);

            }
        }

        public async Task InsertPedidoAsync
            (int idpedido, int precio, DateTime fecha, string itemsmenu, int idmesa, int idmenu, int cantidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Pedido/Create";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Pedido pedido = new Pedido();
                pedido.IdPedido = idpedido;
                pedido.Precio = precio;
                pedido.Fecha = fecha;
                pedido.ItemsMenu = itemsmenu;
                
                pedido.IdMesa = idmesa;
                pedido.IdMenu = idmenu;
                pedido.Cantidad = cantidad;

                string json = JsonConvert.SerializeObject(pedido);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);

            }
        }

        public async Task UpdatePedidoAsync
           (int idpedido, int precio, DateTime fecha, string itemsmenu, int idmesa, int idmenu, int cantidad)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/Pedido/" + idpedido;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Pedido pedido =
                    new Pedido
                    {
                      IdPedido = idpedido,
                      Precio = precio,
                      Fecha = fecha,
                      ItemsMenu = itemsmenu,
                      IdMenu = idmenu,
                      IdMesa = idmesa,
                      Cantidad  = cantidad
                    };

                string json = JsonConvert.SerializeObject(pedido);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync(request, content);
            }
        }




    }
}