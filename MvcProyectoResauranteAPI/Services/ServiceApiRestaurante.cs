using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuggetRestauranteXZX.Models;
using System.Net.Http.Headers;
using System.Text;

namespace MvcProyectoResauranteAPI.Services
{
    public class ServiceApiRestaurante
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;

        //SECRET KEY
        public ServiceApiRestaurante(SecretClient secretclient)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");


            //SECRET KEY
            KeyVaultSecret keyVaultSecret =
                       secretclient.GetSecretAsync("ApiProyectoRestaurante").Result.Value;
            this.UrlApi =
            keyVaultSecret.Value;
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


        private async Task<T> CallApiAsync<T>(string request, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add
                    ("Authorization", "bearer " + token);
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    T data =
                        await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<Usuario> GetPerfilUsuarioAsync
           (string token)
        {
            string request = "/api/usuarios/perfilusuario";
            Usuario usuario = await
                this.CallApiAsync<Usuario>(request, token);
            return usuario;
        }



        #region LOGIN
        public async Task<string> GetTokenAsync(string username, string password)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/auth/login";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                LoginModel model = new LoginModel
                {
                    UserName = username,
                    Password = password
                };

                string jsonModel = JsonConvert.SerializeObject(model);
                StringContent content =
                    new StringContent(jsonModel, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(request, content);
                if (response.IsSuccessStatusCode)
                {
                    string data =
                        await response.Content.ReadAsStringAsync();
                    JObject jsonObject = JObject.Parse(data);
                    string token =
                        jsonObject.GetValue("response").ToString();
                    return token;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task GetRegisterUserAsync
          (string nombre, string email, string password, string imagen)
        {

            using (HttpClient client = new HttpClient())
            {
                string request = "/api/auth/register";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                Usuario usuario = new Usuario();
                usuario.IdUsuario = 0;
                usuario.Nombre = nombre;

                usuario.Email = email;
                usuario.Password = password;
                usuario.ImagenPerfil = imagen;

                string json = JsonConvert.SerializeObject(usuario);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(request, content);
            }
        }

        #endregion


        #region ITEM MENU
        public async Task<List<ItemMenu>> GetItemMenuAsync()
        {
            string request = "/api/ItemMenu";
            List<ItemMenu> itemMenus =
                await this.CallApiAsync<List<ItemMenu>>(request);
            return itemMenus;
        }

        public async Task<List<ItemMenu>> GetItemMenuCategoriaAsync(string categoria)
        {
            string request = "/api/ItemMenu/GetItemMenuCategoria/" + categoria;
            List<ItemMenu> itemMenus =
                await this.CallApiAsync<List<ItemMenu>>(request);
            return itemMenus;
        }


        public async Task<ItemMenu> FindItemMenuAsync(int idmenu)
        {
            string request = "/api/ItemMenu/FindItemMenu/" + idmenu;
            ItemMenu menu =
                await this.CallApiAsync<ItemMenu>(request);
            return menu;
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
        #endregion


        #region MESA

        public async Task<List<Mesa>> GetMesaAsync()
        {
            string request = "/api/Mesa";
            List<Mesa> Mesa =
                await this.CallApiAsync<List<Mesa>>(request);
            return Mesa;
        }

        public async Task<Mesa> FindMesaAsync(int idmesa)
        {
            string request = "/api/Mesa/" + idmesa;
            Mesa mesa =
                await this.CallApiAsync<Mesa>(request);
            return mesa;
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

        public async Task<Mesa> GetMesaOcupadaAsync(int idmesa)
        {
            string request = "/api/Mesa/MesaOcupada/" + idmesa;
            Mesa Mesa =
                await this.CallApiAsync<Mesa>(request);
            return Mesa;
        }

        public async Task<Mesa> GetMesaLibreAsync(int idmesa)
        {
            string request = "/api/Mesa/MesaLibre/" + idmesa;
            Mesa Mesa =
                await this.CallApiAsync<Mesa>(request);
            return Mesa;
        }


        #endregion

        #region PEDIDO
        public async Task<List<Pedido>> GetPedidoAsync()
        {
            string request = "/api/Pedido/GetPedido";
            List<Pedido> Pedido =
                await this.CallApiAsync<List<Pedido>>(request);
            return Pedido;
        }

        public async Task<List<Pedido>> GetPedidoMesaAsync(int idmesa)
        {
            string request = "/api/Pedido/GetPedidoMesa/" + idmesa;
            List<Pedido> Pedido =
                await this.CallApiAsync<List<Pedido>>(request);
            return Pedido;
        }



        public async Task<Pedido> FindPedidoAsync(int idpedido)
        {
            string request = "/api/Pedido/FindPedido/" + idpedido;
            Pedido pedido =
                await this.CallApiAsync<Pedido>(request);
            return pedido;
        }

        public async Task<Pedido> BuscarPedidoPagar(int idmesa)
        {
            string request = "/api/Pedido/BuscarPedidoPagar/" + idmesa;
            Pedido pedido =
                await this.CallApiAsync<Pedido>(request);
            return pedido;
        }

        public async Task<Pedido> PagarPedido(int idmesa)
        {
            string request = "/api/Pedido/PagarPedido/" + idmesa;
            Pedido pedido =
                await this.CallApiAsync<Pedido>(request);
            return pedido;
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
            (int idpedido, decimal precio, DateTime fecha, string itemsmenu, int idmesa, int idmenu, int cantidad)
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
           (int idpedido, decimal precio, DateTime fecha, string itemsmenu, int idmesa, int idmenu, int cantidad)
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
                        Cantidad = cantidad
                    };

                string json = JsonConvert.SerializeObject(pedido);

                StringContent content =
                    new StringContent(json, Encoding.UTF8, "application/json");

                await client.PutAsync(request, content);
            }
        }

        decimal total = 0;
        public decimal SumaPrecio(decimal precio)
        {
            total = +precio;
            return total;

        }

        #endregion


    }
}
