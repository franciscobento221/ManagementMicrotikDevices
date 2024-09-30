using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Runtime.InteropServices;
using static LTI.Form1;
using System.Xml.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace LTI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tabControl1.Hide();
            checkedListBox1.Hide();
            buttonGetInterfaces.Hide();
            buttonGetWlan.Hide();
            groupBox1.Hide();
            buttonGETBridge.Enabled = false;
            buttonCriarBridge.Enabled = false;
            buttonCriarPerfil.Enabled = false;
            buttonCriarRotaEstatica.Enabled = false;
            buttonCriarEnderecoIP.Enabled = false;
            buttonCriarServidorDHCP.Enabled = false;
            buttonCriarDNS.Enabled = false;
            buttonCriarEnderecoIP.Enabled = false;
            buttonGETPerfil.Enabled = false;
            buttonGETRotas.Enabled = false;
            buttonGETIP.Enabled = false;
            buttonGETDHCP.Enabled = false;
            buttonGETWifi.Enabled = false;
            buttonGetTunnel.Enabled = false;





        }
        #region CLASS
        public class Interface
        {
            [JsonProperty("name")]
            public string name { get; set; }
        }

        public class Port
        {
            [JsonProperty("interface")]
            public string interfaceActual { get; set; }

            [JsonProperty("bridge")]
            public string bridge { get; set; }
        }



        public class PerfilSeguranca
        {
            [JsonProperty("name")]
            public string name { get; set; }

            [JsonProperty("wpa2-pre-shared-key")]
            public string password { get; set; }

        }

        public class RotaEstatica
        {

            [JsonProperty("dst-address")]
            public string enderenco { get; set; }

            [JsonProperty("gateway")]
            public string gateway { get; set; }

        }

        public class RedeWireless
        {

            [JsonProperty("name")]
            public string nome { get; set; }


            [JsonProperty("disabled")]
            public string desligado { get; set; }

            [JsonProperty("ssid")]
            public string ssid { get; set; }

            [JsonProperty("mtu")]
            public string mtu { get; set; }

            [JsonProperty("security-profile")]
            public string securityProfile { get; set; }


        }


        public class EnderecoIP
        {

            [JsonProperty("address")]
            public string enderenco { get; set; }


            [JsonProperty("interface")]
            public string interfaceActual { get; set; }

        }


        public class Wireguard
        {

            [JsonProperty("name")]
            public string nome { get; set; }

            [JsonProperty("listen-port")]
            public string port { get; set; }

            [JsonProperty("public-key")]
            public string publicKey { get; set; }

            [JsonProperty("disabled")]
            public string disabled { get; set; }
        }


        public class Peer
        {

            [JsonProperty("interface")]
            public string interfaceActual { get; set; }

            [JsonProperty("allowed-address")]
            public string allowedAddress { get; set; }


        }

        public class ServidorDHCP
        {

            [JsonProperty("name")]
            public string nome { get; set; }

            [JsonProperty("interface")]
            public string interfaceActual { get; set; }
            [JsonProperty("address-pool")]
            public string addressPool { get; set; }

        }

        public class ServidorDNS
        {

            [JsonProperty("name")]
            public string nome { get; set; }

            [JsonProperty("address")]
            public string address { get; set; }

        }

        public class AddressPool
        {

            [JsonProperty("name")]
            public string nome { get; set; }


        }

        public class Identificador
        {
            [JsonProperty(".id")]
            public string Id { get; set; }
        }

        #endregion


        #region GLOBAl
        static class Global
        {
            public static string ip = "";
            public static string username = "";
            public static string password = "";


            public static string GlobalVarIp
            {
                get { return ip; }
                set { ip = value; }
            }

            public static string GlobalVarUsername
            {
                get { return username; }
                set { username = value; }
            }

            public static string GlobalVarPassword
            {
                get { return password; }
                set { password = value; }
            }
        }

        public string GetCredentials()
        {
            Global.ip = textBoxIp.Text;
            Global.username = textBoxUsername.Text;
            Global.password = textBoxPassword.Text;

            string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(Global.username + ":" + Global.password));

            return credentials;
        }

        static string ExtractId(string input)
        {
            // Remove square brackets [ and ]
            input = input.Trim('[', ']', '}');

            // Remove double quotes "
            input = input.Replace("\"", "");

            // Find the index of the substring ":"
            int index = input.IndexOf(":");

            // Extract the substring starting from the character after ":"
            string id = input.Substring(index + 1).Trim();

            // Remove * prefix from the value
            id = id.TrimStart('*');

            return id;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointTeste = "http://" + Global.ip + "/rest/system/resource";

            // Create a WebClient instance to perform the GET request
            WebClient client = new WebClient();

            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;


            try
            {

                // Perform the GET request and store the response
                string response = client.DownloadString(endpointTeste);
                //Console.WriteLine(response);
                // Display the response in the ListView

                MessageBox.Show("CREDENCIAIS CORRETAS");
                tabControl1.Show();
                checkedListBox1.Show();
                buttonGetInterfaces.Show();
                buttonGetWlan.Show();
                groupBox1.Show();

                textBoxIp.ReadOnly = true;
                textBoxUsername.ReadOnly = true;
                textBoxPassword.ReadOnly = true;



            }
            catch (Exception)
            {
                //MessageBox.Show("Error: " + ex.Message);
                MessageBox.Show("CREDENCIAIS INCORRETAS");
                textBoxUsername.Clear();
                textBoxPassword.Clear();
            }
            finally
            {
                // Dispose the WebClient to free up resources
                client.Dispose();
            }

        }

        private void buttonDisconectar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("LIGAÇÃO TERMINADA");
            tabControl1.Hide();
            checkedListBox1.Hide();
            buttonGetInterfaces.Hide();
            buttonGetWlan.Hide();
            groupBox1.Hide();

            textBoxIp.ReadOnly = false;
            textBoxUsername.ReadOnly = false;
            textBoxPassword.ReadOnly = false;

            textBoxIp.Text = "";
            textBoxUsername.Text = "";
            textBoxPassword.Text = "";


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #endregion



        #region INTERFACES
        //####################-INTERFACES-##################################################################################################################################################################################
        private void buttonGetInterfaces_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetAll = "http://" + textBoxIp.Text + "/rest/interface";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            checkedListBox1.Items.Clear();



            try
            {
                // Perform the GET request and store the response
                string response = client.DownloadString(endpointGetAll);


                var obj = JsonConvert.DeserializeObject(response);

                Console.WriteLine(obj);



                // Deserialize the JSON array into a collection of Interface objects
                List<Interface> interfaces = JsonConvert.DeserializeObject<List<Interface>>(obj.ToString());

                // Access the value of the "default-name" parameter for each Interface object
                foreach (var iface in interfaces)
                {
                    checkedListBox1.ClearSelected();
                    checkedListBox1.Items.Add(iface.name);

                }

            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Dispose the HttpClient to free up resources
                client.Dispose();
            }
        }

        private void buttonGetWlan_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetWlan = "http://" + textBoxIp.Text + "/rest/interface?type=wlan";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            checkedListBox1.Items.Clear();



            try
            {
                // Perform the GET request and store the response
                string response = client.DownloadString(endpointGetWlan);


                var obj = JsonConvert.DeserializeObject(response);

                Console.WriteLine(obj);



                // Deserialize the JSON array into a collection of Interface objects
                List<Interface> interfaces = JsonConvert.DeserializeObject<List<Interface>>(obj.ToString());

                // Access the value of the "name" parameter for each Interface object
                foreach (var iface in interfaces)
                {


                    checkedListBox1.Items.Add(iface.name);

                }

            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Dispose the HttpClient to free up resources
                client.Dispose();
            }

        }
        #endregion


        #region BRIDGE
        //####################---BRIDGE---##################################################################################################################################################################################


        private void buttonCriarBridge_Click(object sender, EventArgs e)
        {

            string credentials = GetCredentials();

            string endpointCreateBridge = "http://" + textBoxIp.Text + "/rest/interface/bridge/add";
            string endpointAssociatePort = "http://" + textBoxIp.Text + "/rest/interface/bridge/port/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";


            string jsonContentBridge = @"{
                ""name"": """ + textBoxNomeBridge.Text + @"""
                
            }";




            try
            {
                string responseBridge = client.UploadString(endpointCreateBridge, "POST", jsonContentBridge);
                Console.WriteLine("Response: " + responseBridge);
                MessageBox.Show("BRIDGE CRIADA !");

                if (responseBridge.Contains("ret"))
                {
                    try
                    {
                        List<string> checkedValues = new List<string>();//VAI BUSCAR AS PORTAS SELECIONADAS, PARA CRIAR A BRIDGE (NAO IMPLEMENTADO)
                        foreach (var item in checkedListBox1.CheckedItems)
                        {
                            checkedValues.Add(item.ToString());
                        }
                        Console.WriteLine("All checked items:");
                        foreach (var item in checkedValues)
                        {

                            string jsonContentPort = @"{
                                ""bridge"": """ + textBoxNomeBridge.Text + @""",
                                ""interface"": """ + item + @"""
                            }";


                            string responsePort = client.UploadString(endpointAssociatePort, "POST", jsonContentPort);

                        }

                        MessageBox.Show("PORTO ASSOCIADO A BRIDGE COM SUCESSO!");

                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                        MessageBox.Show("NÃO FOI POSSIVEL ASSOCIAR PORTO A BRIDGE !");
                    }
                }
            }
            catch (WebException ex)
            {
                // Handle any exceptions
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO CRIAR BRIDGE !");
            }



        }

        private void buttonListarBridge_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetBridge = "http://" + textBoxIp.Text + "/rest/interface/bridge/port";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            checkedListBox2.Items.Clear();



            try
            {
                // Perform the GET request and store the response
                string response = client.DownloadString(endpointGetBridge);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);



                // Deserialize the JSON array into a collection of Interface objects
                List<Port> interfaces = JsonConvert.DeserializeObject<List<Port>>(obj.ToString());

                var groupedInterfaces = interfaces.GroupBy(i => i.bridge)
                                 .Select(g => $"{g.Key} -> {string.Join(", ", g.Select(i => i.interfaceActual))}");

                foreach (var item in groupedInterfaces)
                {
                    checkedListBox2.Items.Add(item);
                }
                // Get the root element of the JSON document
                //JsonElement root = jsonDoc.RootElement;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                // Dispose the HttpClient to free up resources
                client.Dispose();
            }
        }

        private void buttonEliminarBridge_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json"; // Adjust content type as needed

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox2.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }


            if (checkedListBox2.CheckedItems.Count == 0)
            {
                MessageBox.Show("SELECIONE UMA BRIDGE PARA ELIMINAR!");
            }
            else
            {


                foreach (var item in checkedValues)
                {
                    try
                    {
                        string[] parts = item.Split(new string[] { "->" }, StringSplitOptions.None);

                        string bridge = parts[0].Trim();

                        string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/bridge/port?.proplist=.id&bridge=" + bridge + "";
                        string response = client.DownloadString(endpointGetId);
                        List<Identificador> bridgePortIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                        string[] ids = new string[bridgePortIds.Count];
                        for (int i = 0; i < bridgePortIds.Count; i++)
                        {
                            ids[i] = bridgePortIds[i].Id;
                        }

                        foreach (string id in ids)
                        {
                            string endpointDeleteBridgePort = "http://" + textBoxIp.Text + "/rest/interface/bridge/port/" + id + "";
                            client.UploadString(endpointDeleteBridgePort, "DELETE", string.Empty);
                        }




                        string endpointDeleteBridge = "http://" + textBoxIp.Text + "/rest/interface/bridge/" + bridge + "";
                        client.UploadString(endpointDeleteBridge, "DELETE", string.Empty);
                        MessageBox.Show("BRIDGE ELIMINADA !");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("ERRO AO ELIMINAR BRIDGE!");

                    }
                    finally
                    {
                        // Dispose the HttpClient to free up resources
                        client.Dispose();

                    }


                }
            }





        }

        private void buttonRefreshEditBridge_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetBridge = "http://" + textBoxIp.Text + "/rest/interface?type=bridge";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            comboBox1.Items.Clear();
            try
            {
                string response = client.DownloadString(endpointGetBridge);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<Interface> interfaces = JsonConvert.DeserializeObject<List<Interface>>(obj.ToString());

                foreach (var iface in interfaces)
                {
                    comboBox1.Items.Add(iface.name);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }


        }

        private void buttonEditarBridge_Click(object sender, EventArgs e)
        {
            string bridgeAEditar = comboBox1.Text;
            string novoNomeBridge = textBoxNovoNomeBridge.Text;

            string credentials = GetCredentials();
            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/bridge?.proplist=.id&name=" + bridgeAEditar + "";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<Identificador> bridgePortIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[bridgePortIds.Count];
                for (int i = 0; i < bridgePortIds.Count; i++)
                {
                    ids[i] = bridgePortIds[i].Id;
                }

                foreach (string id in ids)
                {



                    try
                    {
                        string endpointAssociatePort = "http://" + textBoxIp.Text + "/rest/interface/bridge/port/add";
                        string endpointGetIdBridgePort = "http://" + textBoxIp.Text + "/rest/interface/bridge/port?.proplist=.id&bridge=" + bridgeAEditar + "";
                        string responseBridgePort = client.DownloadString(endpointGetIdBridgePort);
                        List<Identificador> bridgePortIdsToDelete = JsonConvert.DeserializeObject<List<Identificador>>(responseBridgePort);


                        string[] idsToDelete = new string[bridgePortIdsToDelete.Count];
                        for (int i = 0; i < bridgePortIdsToDelete.Count; i++)
                        {
                            idsToDelete[i] = bridgePortIdsToDelete[i].Id;
                        }

                        //if (bridgePortIdsToDelete.Count != checkedListBox1.CheckedItems.Count)
                        //{
                        //}

                        foreach (string idToDelete in idsToDelete)
                        {
                            string endpointDeleteBridgePort = "http://" + textBoxIp.Text + "/rest/interface/bridge/port/" + idToDelete + "";
                            client.UploadString(endpointDeleteBridgePort, "DELETE", string.Empty);
                        }


                        List<string> checkedValues = new List<string>();
                        foreach (var item in checkedListBox1.CheckedItems)
                        {
                            checkedValues.Add(item.ToString());
                        }
                        Console.WriteLine("All checked items:");
                        foreach (var item in checkedValues)
                        {

                            string jsonContentPortEdit = @"{
                                ""bridge"": """ + novoNomeBridge + @""",
                                ""interface"": """ + item + @"""
                            }";

                            string responsePort = client.UploadString(endpointAssociatePort, "POST", jsonContentPortEdit);
                            MessageBox.Show("INTERFACES DA BRIDGE EDITADAS !");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("ERRO AO EDITAR INTERFACE DA BRIDGE !");

                    }

                    try
                    {
                        string jsonContentBridge = @"{
                                ""name"": """ + novoNomeBridge + @"""
                            }";

                        string endpointEditBridge = "http://" + textBoxIp.Text + "/rest/interface/bridge/" + id + "";
                        client.UploadString(endpointEditBridge, "PATCH", jsonContentBridge);
                        MessageBox.Show("NOME DA BRIDGE EDITADO !");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        MessageBox.Show("ERRO AO EDITAR NOME BRIDGE !");
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO EDITAR NOME BRIDGE  !");
            }
            finally
            {
                client.Dispose();
                comboBox1.Text = "";
                comboBox1.Items.Clear();
                textBoxNovoNomeBridge.Clear();

            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGETBridge.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGETBridge.Enabled = false;

            }
        }

        private void buttonGETBridge_Click(object sender, EventArgs e)
        {


            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string bridgeAEditar = comboBox1.Text;

            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/bridge?.proplist=.id&name=" + bridgeAEditar + "";


            string responseID = client.DownloadString(endpointGetId);
            var obj = JsonConvert.DeserializeObject(responseID);

            List<Identificador> bridgeId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[bridgeId.Count];
            for (int i = 0; i < bridgeId.Count; i++)
            {
                ids[i] = bridgeId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointBridge = "http://" + textBoxIp.Text + "/rest/interface/bridge/" + id + "";
                string responseBridge = client.DownloadString(endpointBridge);

                Interface bridge = JsonConvert.DeserializeObject<Interface>(responseBridge);

                textBoxNovoNomeBridge.Text = bridge.name;


            }

            buttonGetInterfaces.PerformClick();

            string endpointGETBridgePorts = "http://" + textBoxIp.Text + "/rest/interface/bridge/port?.proplist=interface&bridge=" + bridgeAEditar + "";

            string responseBridgePorts = client.DownloadString(endpointGETBridgePorts);

            //Port ports = JsonConvert.DeserializeObject<Port>(responseBridgePorts);
            List<Port> portsBridge = JsonConvert.DeserializeObject<List<Port>>(responseBridgePorts);

            string[] ports = new string[portsBridge.Count];

            for (int i = 0; i < portsBridge.Count; i++)
            {
                ports[i] = portsBridge[i].interfaceActual;
            }

            foreach (string port in ports)
            {
                string interfaceName = port; // Extract the interface name
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    // Check if the interface name exists in your list
                    if (checkedListBox1.Items[i].ToString() == interfaceName)
                    {
                        // If it exists, check the item in the CheckedListBox
                        checkedListBox1.SetItemChecked(i, true);
                        break; // Exit the loop once found
                    }
                }
            }

        }

        #endregion


        #region PERFIL

        //####################---PERFIL---##################################################################################################################################################################################


        private void buttonListarPerfil_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetPerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            checkedListBox3.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetPerfil);
                var obj = JsonConvert.DeserializeObject(response);
                //Console.WriteLine(obj
                List<PerfilSeguranca> perfis = JsonConvert.DeserializeObject<List<PerfilSeguranca>>(obj.ToString());

                foreach (var iface in perfis)
                {

                    checkedListBox3.Items.Add(iface.name);

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonCriarPerfil_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointCreatePerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";



            //NOME NÃO PODE TER ESPAÇO !! 
            string jsonContent = @"{
                ""name"": """ + textBoxNomePerfil.Text + @""",
                ""authentication-types"": ""wpa2-psk"",
                ""mode"": ""dynamic-keys"",
                ""group-ciphers"": ""aes-ccm"",
                ""unicast-ciphers"": ""aes-ccm"",
                ""wpa2-pre-shared-key"": """ + textBoxPasswordPerfil.Text + @"""
            }";


            string response = "";
            try
            {
                response = client.UploadString(endpointCreatePerfil, "POST", jsonContent);
                Console.WriteLine("Response: " + response);
                MessageBox.Show("PERFIL CRIADO !");
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO CRIAR PERFIL!");
            }
            finally
            {

                client.Dispose();

            }
        }

        private void buttonEliminarPerfil_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox3.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }

            if (checkedListBox3.CheckedItems.Count == 0)
            {
                MessageBox.Show("SELECIONE UM PERFIL PARA ELIMINAR !");
            }
            else
            {
                foreach (var item in checkedValues)
                {
                    try
                    {
                        string endpointDeletePerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles/" + item + "";
                        client.UploadString(endpointDeletePerfil, "DELETE", string.Empty);
                        MessageBox.Show("PERFIL ELIMINADO !");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("ERRO AO ELIMINAR PERFIL !");
                    }
                    finally
                    {
                        client.Dispose();


                    }




                }
            }
        }

        private void buttonRefreshPerfil_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetPerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            comboBoxPerfis.Items.Clear();
            try
            {
                string response = client.DownloadString(endpointGetPerfil);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<PerfilSeguranca> perfis = JsonConvert.DeserializeObject<List<PerfilSeguranca>>(obj.ToString());

                foreach (var iface in perfis)
                {
                    comboBoxPerfis.Items.Add(iface.name);

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonEditarPerfil_Click(object sender, EventArgs e)
        {
            string perfilAEditar = comboBoxPerfis.Text;
            string novoNomePerfil = textBoxNovoNomePerfil.Text;
            string novaPasswordPerfil = textBoxNovaPasswordPerfil.Text;

            string credentials = GetCredentials();
            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles?.proplist=.id&name=" + perfilAEditar + "";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentEditPerfil = @"{
                ""name"": """ + novoNomePerfil + @""",
                ""wpa2-pre-shared-key"": """ + novaPasswordPerfil + @"""
            }";


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<Identificador> perfilIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[perfilIds.Count];
                for (int i = 0; i < perfilIds.Count; i++)
                {
                    ids[i] = perfilIds[i].Id;
                }

                foreach (string id in ids)
                {
                    string endpointEditPerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles/" + id + "";
                    client.UploadString(endpointEditPerfil, "PATCH", jsonContentEditPerfil);
                }
                MessageBox.Show("PERFIL EDITADO !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
                comboBoxPerfis.Items.Clear();
                comboBoxPerfis.Text = "";
                textBoxNovoNomePerfil.Clear();
                textBoxNovaPasswordPerfil.Clear();

            }
        }

        private void buttonGETPerfil_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string perfilAEditar = comboBoxPerfis.Text;

            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles?.proplist=.id&name=" + perfilAEditar + "";


            string responseID = client.DownloadString(endpointGetId);
            var obj = JsonConvert.DeserializeObject(responseID);

            List<Identificador> perfilId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[perfilId.Count];
            for (int i = 0; i < perfilId.Count; i++)
            {
                ids[i] = perfilId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointPerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles/" + id + "";
                string responsePerfil = client.DownloadString(endpointPerfil);

                PerfilSeguranca perfil = JsonConvert.DeserializeObject<PerfilSeguranca>(responsePerfil);

                textBoxNovoNomePerfil.Text = perfil.name;
                textBoxNovaPasswordPerfil.Text = perfil.password;


            }
        }


        #endregion


        #region ROTAS ESTATICAS

        //####################---ROTAS ESTATICAS---##################################################################################################################################################################################


        private void buttonListarRotas_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetRotas = "http://" + textBoxIp.Text + "/rest/ip/route";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            checkedListBox4.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetRotas);
                var obj = JsonConvert.DeserializeObject(response);
                List<RotaEstatica> perfis = JsonConvert.DeserializeObject<List<RotaEstatica>>(obj.ToString());

                foreach (var iface in perfis)
                {

                    checkedListBox4.Items.Add("" + iface.enderenco + "::" + iface.gateway + "");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }



        private void buttonCriarRotaEstatica_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointCreateRota = "http://" + textBoxIp.Text + "/rest/ip/route/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";




            string jsonContent = @"{
                ""dst-address"": """ + textBoxEnderecoRota.Text + @""",
                ""gateway"": """ + textBoxGatewayRota.Text + @"""
                 
            }";


            try
            {
                string response = client.UploadString(endpointCreateRota, "POST", jsonContent);
                Console.WriteLine("Response: " + response);
                MessageBox.Show("ROTA CRIADA !");
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO CRIAR ROTA !");
            }
            finally
            {
                client.Dispose();

            }
        }




        private void buttonEliminarRotaEstatica_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox4.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }


            if (checkedListBox4.CheckedItems.Count == 0)
            {
                MessageBox.Show("SELECIONE UMA ROTA PARA ELIMINAR!");
            }
            else
            {
                foreach (var item in checkedValues)
                {
                    try
                    {
                        string[] parts = item.Split(new string[] { "::" }, StringSplitOptions.None);

                        string endereco = parts[0];
                        string gateway = parts[1];


                        string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/route?.proplist=.id&dst-address=" + endereco + "&gateway=" + gateway + "";
                        string response = client.DownloadString(endpointGetId);


                        string id = ExtractId(response);

                        Console.WriteLine(id);


                        string endpointDeleteRota = "http://" + textBoxIp.Text + "/rest/ip/route/*" + id + "";
                        client.UploadString(endpointDeleteRota, "DELETE", string.Empty);
                        MessageBox.Show("ROTA ELIMINADA!");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("ERRO AO ELIMINAR ROTA ");
                    }
                    finally
                    {
                        client.Dispose();

                    }




                }
            }
        }

        private void buttonRefreshRota_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetRota = "http://" + textBoxIp.Text + "/rest/ip/route";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            comboBoxRotas.Items.Clear();
            try
            {
                string response = client.DownloadString(endpointGetRota);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<RotaEstatica> rotas = JsonConvert.DeserializeObject<List<RotaEstatica>>(obj.ToString());

                foreach (var iface in rotas)
                {
                    comboBoxRotas.Items.Add("" + iface.enderenco + "::" + iface.gateway + "");

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonEditarRota_Click(object sender, EventArgs e)
        {
            string rotaAEditar = comboBoxRotas.Text;
            string novoEndereco = textBoxNovoEndereco.Text;
            string novoGateway = textBoxNovoGateway.Text;

            string[] parts = rotaAEditar.Split(new string[] { "::" }, StringSplitOptions.None);
            string endereco = parts[0];
            string gateway = parts[1];

            string credentials = GetCredentials();
            string endpointGetId = "http:/" + textBoxIp.Text + "/rest/ip/route?.proplist=.id&dst-address=" + endereco + "&gateway=" + gateway + "";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentEditRota = @"{
                ""dst-address"": """ + novoEndereco + @""",
                ""gateway"": """ + novoGateway + @"""
                 
            }";


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<Identificador> rotaIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[rotaIds.Count];
                for (int i = 0; i < rotaIds.Count; i++)
                {
                    ids[i] = rotaIds[i].Id;
                }

                foreach (string id in ids)
                {
                    string endpointEditRota = "http://" + textBoxIp.Text + "/rest/ip/route/" + id + "";
                    client.UploadString(endpointEditRota, "PATCH", jsonContentEditRota);
                }
                MessageBox.Show("ROTA EDITADA!");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO EDITAR ROTA!");
            }
            finally
            {
                client.Dispose();
                comboBoxRotas.Items.Clear();
                comboBoxRotas.Text = "";
                textBoxNovoEndereco.Clear();
                textBoxNovoGateway.Clear();
            }
        }

        private void buttonGETRotas_Click(object sender, EventArgs e) //ERRO SE A COMBO BOX ESTIVER VAZIA !!! 
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string rotaAEditar = comboBoxRotas.Text;

            string[] parts = rotaAEditar.Split(new string[] { "::" }, StringSplitOptions.None);
            string endereco = parts[0];
            string gateway = parts[1];

            string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/route?.proplist=.id&dst-address=" + endereco + "&gateway=" + gateway + "";


            string responseID = client.DownloadString(endpointGetId);
            var obj = JsonConvert.DeserializeObject(responseID);

            List<Identificador> rotaId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[rotaId.Count];
            for (int i = 0; i < rotaId.Count; i++)
            {
                ids[i] = rotaId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointRota = "http://" + textBoxIp.Text + "/rest/ip/route/" + id + "";
                string responseRota = client.DownloadString(endpointRota);

                RotaEstatica rota = JsonConvert.DeserializeObject<RotaEstatica>(responseRota);

                textBoxNovoEndereco.Text = rota.enderenco;
                textBoxNovoGateway.Text = rota.gateway;


            }
        }

        #endregion



        #region ENDERECOS IP

        //####################---ENDERECOS IP---##################################################################################################################################################################################


        private void buttonListarEnderecosIP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetIPs = "http://" + textBoxIp.Text + "/rest/ip/address";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            checkedListBox5.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetIPs);
                var obj = JsonConvert.DeserializeObject(response);

                List<EnderecoIP> enderecoIPs = JsonConvert.DeserializeObject<List<EnderecoIP>>(obj.ToString());

                foreach (var iface in enderecoIPs)
                {

                    checkedListBox5.Items.Add("" + iface.enderenco + "->" + iface.interfaceActual + "");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonCriarEnderecoIP_Click(object sender, EventArgs e) //DIFERENÇA ENTRE ACTUAL-INTERFACE E INTERFACE ? ? 
        {
            string credentials = GetCredentials();

            string endpointCreateEndercoIP = "http://" + textBoxIp.Text + "/rest/ip/address/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";


            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }
            Console.WriteLine("All checked items:");
            foreach (var item in checkedValues)
            {
                Console.WriteLine(item);


                string jsonContent = @"{
                ""interface"": """ + item + @""",
                ""address"": """ + textBoxEnderecoIP.Text + @"""
                 
            }";


                try
                {
                    string response = client.UploadString(endpointCreateEndercoIP, "POST", jsonContent);
                    Console.WriteLine("Response: " + response);
                    MessageBox.Show("ENDEREÇO CRIADO !");
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    MessageBox.Show("ERRO AO CRIAR ENDEREÇO !!");
                }
            }
        }

        private void buttonEliminarEndereçoIP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox5.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }

            if (checkedListBox5.CheckedItems.Count == 0)
            {
                MessageBox.Show("SELECIONE UM ENDEREÇO IP PARA ELIMINAR!");
            }
            else
            {

                foreach (var item in checkedValues)
                {
                    try
                    {
                        string[] parts = item.Split(new string[] { "->" }, StringSplitOptions.None);

                        string endereco = parts[0];
                        string interfaceActual = parts[1];


                        string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/address?.proplist=.id&address=" + endereco + "&interface=" + interfaceActual + "";
                        string response = client.DownloadString(endpointGetId);


                        string id = ExtractId(response);

                        Console.WriteLine(id);


                        string endpointDeleteEnderecoIP = "http://" + textBoxIp.Text + "/rest/ip/address/*" + id + "";
                        client.UploadString(endpointDeleteEnderecoIP, "DELETE", string.Empty);

                        MessageBox.Show("ENDEREÇO IP ELIMINADO !");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("ERRO AO ELIMINAR ENDERÇO IP");
                    }
                    finally
                    {
                        client.Dispose();

                    }


                }
            }
        }

        private void buttonRefreshIP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetIPs = "http://" + textBoxIp.Text + "/rest/ip/address";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            comboBoxIPs.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetIPs);
                var obj = JsonConvert.DeserializeObject(response);

                List<EnderecoIP> enderecoIPs = JsonConvert.DeserializeObject<List<EnderecoIP>>(obj.ToString());

                foreach (var iface in enderecoIPs)
                {

                    comboBoxIPs.Items.Add("" + iface.enderenco + "->" + iface.interfaceActual + "");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonEditarIP_Click(object sender, EventArgs e)
        {
            string ipAEditar = comboBoxIPs.Text;
            string novoIp = textBoxNovoIP.Text;
            string interfaceActual = checkedListBox1.SelectedItem.ToString();

            string[] parts = ipAEditar.Split(new string[] { "->" }, StringSplitOptions.None);
            string ip = parts[0];
            string gateway = parts[1];

            string credentials = GetCredentials();
            string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/address?.proplist=.id&address=" + ip + "&interface=" + gateway + "";

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentEditIp = @"{
                ""address"": """ + novoIp + @""",
                ""interface"": """ + interfaceActual + @"""
                 
            }";


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<Identificador> ipIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[ipIds.Count];
                for (int i = 0; i < ipIds.Count; i++)
                {
                    ids[i] = ipIds[i].Id;
                }

                foreach (string id in ids)
                {
                    string endpointEditIp = "http://" + textBoxIp.Text + "/rest/ip/address/" + id + "";
                    client.UploadString(endpointEditIp, "PATCH", jsonContentEditIp);
                }
                MessageBox.Show("ENDEREÇO IP EDITADO !");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO EDITAR ENDEREÇO IP !");
            }
            finally
            {
                client.Dispose();
                comboBoxIPs.Text = "";
                comboBoxIPs.Items.Clear();
                textBoxNovoIP.Text = "";
                checkedListBox1.Items.Clear();

            }
        }

        private void buttonGETIP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string ipAEditar = comboBoxIPs.Text;

            string[] parts = ipAEditar.Split(new string[] { "->" }, StringSplitOptions.None);
            string ip = parts[0];
            string gateway = parts[1];


            string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/address?.proplist=.id&address=" + ip + "&interface=" + gateway + "";


            string responseID = client.DownloadString(endpointGetId);
            var obj = JsonConvert.DeserializeObject(responseID);

            List<Identificador> ipId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[ipId.Count];
            for (int i = 0; i < ipId.Count; i++)
            {
                ids[i] = ipId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointIP = "http://" + textBoxIp.Text + "/rest/ip/address/" + id + "";
                string responseIP = client.DownloadString(endpointIP);

                EnderecoIP enderecoIP = JsonConvert.DeserializeObject<EnderecoIP>(responseIP);

                textBoxNovoIP.Text = enderecoIP.enderenco;

                buttonGetInterfaces.PerformClick();

                string endpointGETInterfaceIP = "http://" + textBoxIp.Text + "/rest/ip/address?.proplist=interface&.id=" + id + "";

                string responseGETInterfaceIPs = client.DownloadString(endpointGETInterfaceIP);

                //Port ports = JsonConvert.DeserializeObject<Port>(responseBridgePorts);
                List<EnderecoIP> enderecoIPInterface = JsonConvert.DeserializeObject<List<EnderecoIP>>(responseGETInterfaceIPs);

                string[] interfaces = new string[enderecoIPInterface.Count];

                for (int i = 0; i < enderecoIPInterface.Count; i++)
                {
                    interfaces[i] = enderecoIPInterface[i].interfaceActual;
                }

                foreach (string interfaceActual in interfaces)
                {
                    string interfaceName = interfaceActual; // Extract the interface name
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        // Check if the interface name exists in your list
                        if (checkedListBox1.Items[i].ToString() == interfaceName)
                        {
                            // If it exists, check the item in the CheckedListBox
                            checkedListBox1.SetItemChecked(i, true);
                            break; // Exit the loop once found
                        }
                    }
                }
            }


        }

        #endregion

        #region SERVIDOR DHCP

        //####################---SERVIDOR DHCP---##################################################################################################################################################################################

        private void buttonListarDHCP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetDHCP = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            checkedListBox6.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetDHCP);
                var obj = JsonConvert.DeserializeObject(response);
                //Console.WriteLine(obj
                List<ServidorDHCP> servidorDHCPs = JsonConvert.DeserializeObject<List<ServidorDHCP>>(obj.ToString());

                foreach (var iface in servidorDHCPs)
                {

                    checkedListBox6.Items.Add("" + iface.nome + " -> " + iface.interfaceActual + " -> " + iface.addressPool + "");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonCriarServidorDHCP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointCreateDCHP = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";


            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }
            Console.WriteLine("All checked items:");
            foreach (var item in checkedValues)
            {
                Console.WriteLine(item);


                string jsonContent = @"{
                ""interface"": """ + item + @""",
                ""name"": """ + textBoxNameDHCP.Text + @""",
                ""address-pool"": """ + comboBoxAddressPool.Text + @""" 
            }";

                MessageBox.Show("SERVIDOR DHCP CRIADO !");
                try
                {
                    string response = client.UploadString(endpointCreateDCHP, "POST", jsonContent);
                    Console.WriteLine("Response: " + response);
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    MessageBox.Show("ERRO AO CRIAR SERVIDOR DHCP !");
                }
                finally
                {
                    client.Dispose();

                }
            }
        }

        private void buttonEliminarDHCP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox6.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }

            if (checkedListBox6.CheckedItems.Count == 0)
            {
                MessageBox.Show("SELECIONE UM SERVIDOR DE DCHP PARA ELIMINAR!");
            }
            else
            {

                foreach (var item in checkedValues)
                {

                    try
                    {
                        string[] parts = item.Split(new string[] { "->" }, StringSplitOptions.None);

                        string nome = parts[0];


                        string endpointDeleteDHCP = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server/" + nome + "";

                        client.UploadString(endpointDeleteDHCP, "DELETE", string.Empty);
                        MessageBox.Show("SERVIDOR DHCP ELIMINADO !");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("ERRO AO ELIMINAR UM SERVIDOR DHCP !");
                    }
                    finally
                    {
                        client.Dispose();

                    }




                }
            }
        }
        private void buttonRefreshDHCP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetIPs = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;


            string endpointGetPools = "http://" + textBoxIp.Text + "/rest/ip/pool";
            WebClient clientPool = new WebClient();
            clientPool.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            comboBoxDHCPs.Items.Clear();
            comboBoxNovaPoolDHCP.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetIPs);
                var obj = JsonConvert.DeserializeObject(response);

                List<ServidorDHCP> servidorDHCPs = JsonConvert.DeserializeObject<List<ServidorDHCP>>(obj.ToString());

                foreach (var iface in servidorDHCPs)
                {

                    comboBoxDHCPs.Items.Add("" + iface.nome + "->" + iface.interfaceActual + "->" + iface.addressPool + "");

                }

                string responsePool = client.DownloadString(endpointGetPools);
                var objPerfil = JsonConvert.DeserializeObject(responsePool);

                List<AddressPool> pools = JsonConvert.DeserializeObject<List<AddressPool>>(objPerfil.ToString());

                foreach (var iface in pools)
                {
                    comboBoxNovaPoolDHCP.Items.Add(iface.nome);

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonEditarDHCP_Click(object sender, EventArgs e)
        {
            string dhcpAEditar = comboBoxDHCPs.Text;
            string novoNome = textBoxNovoNomeDHCP.Text;

            string[] parts = dhcpAEditar.Split(new string[] { "->" }, StringSplitOptions.None);
            string nome = parts[0];
            string interfaceActual = parts[1];
            string addressPool = parts[2];

            string credentials = GetCredentials();
            string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server?.proplist=.id&name=" + nome + "&interface=" + interfaceActual + "&address-pool=" + addressPool + "";

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentEditDHCP = @"{
                ""name"": """ + novoNome + @"""
                
                 
            }";


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                Console.WriteLine(obj);

                List<Identificador> DHCPIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[DHCPIds.Count];
                for (int i = 0; i < DHCPIds.Count; i++)
                {
                    ids[i] = DHCPIds[i].Id;
                }

                foreach (string id in ids)
                {
                    string endpointEditIp = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server/" + id + "";
                    client.UploadString(endpointEditIp, "PATCH", jsonContentEditDHCP);
                }
                MessageBox.Show("SERVIDOR DHCP EDITADO !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO EDITAR DHCP !");
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonGETDHCP_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string dhcpAEditar = comboBoxDHCPs.Text;
            string[] parts = dhcpAEditar.Split(new string[] { "->" }, StringSplitOptions.None);
            string nome = parts[0];
            string interfaceActual = parts[1];
            string addressPool = parts[2];

            string endpointGetId = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server?.proplist=.id&name=" + nome + "&interface=" + interfaceActual + "&address-pool=" + addressPool + "";

            string responseID = client.DownloadString(endpointGetId);
            var obj = JsonConvert.DeserializeObject(responseID);

            List<Identificador> dhcpId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[dhcpId.Count];
            for (int i = 0; i < dhcpId.Count; i++)
            {
                ids[i] = dhcpId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointDHCP = "http://" + textBoxIp.Text + "/rest/ip/dhcp-server/" + id + "";
                string responseDHCP = client.DownloadString(endpointDHCP);

                ServidorDHCP dhcp = JsonConvert.DeserializeObject<ServidorDHCP>(responseDHCP);

                textBoxNovoNomeDHCP.Text = dhcp.nome;
                comboBoxNovaPoolDHCP.Text = dhcp.addressPool;



            }
        }

        private void buttonGETPools_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointGetPools = "http://" + textBoxIp.Text + "/rest/ip/pool";
            WebClient clientPool = new WebClient();
            clientPool.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            comboBoxAddressPool.Items.Clear();
            try
            {

                string responsePool = clientPool.DownloadString(endpointGetPools);
                var objPerfil = JsonConvert.DeserializeObject(responsePool);

                List<AddressPool> pools = JsonConvert.DeserializeObject<List<AddressPool>>(objPerfil.ToString());

                foreach (var iface in pools)
                {
                    comboBoxAddressPool.Items.Add(iface.nome);

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                clientPool.Dispose();
            }
        }

        #endregion


        #region REDE WIRELESS

        //####################---REDE WIRELESS---##################################################################################################################################################################################

        private void buttonRefreshRedesWireless_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetWirelessNetwork = "http://" + textBoxIp.Text + "/rest/interface/wireless";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            richTextBoxStatusRedeWireless.Clear();
            try
            {
                string response = client.DownloadString(enpointGetWirelessNetwork);
                var obj = JsonConvert.DeserializeObject(response);
                List<RedeWireless> redeWirelesses = JsonConvert.DeserializeObject<List<RedeWireless>>(obj.ToString());



                foreach (var iface in redeWirelesses)
                {
                    string statusRede;

                    if (iface.desligado == "true")
                    {
                        statusRede = "OFF";
                    }
                    else
                    {
                        statusRede = "ON";
                    }

                    richTextBoxStatusRedeWireless.AppendText("" + iface.nome + " -> " + statusRede + "\n");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonLigarRedeWireless_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string jsonContent = @"
            {
                ""disabled"": ""false""
            }";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }
            Console.WriteLine("All checked items:");


            foreach (var item in checkedValues)
            {

                string[] parts = item.Split(new string[] { "->" }, StringSplitOptions.None);

                string nome = parts[0];



                string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireless?.proplist=.id&name=" + nome + "";
                string response = client.DownloadString(endpointGetId);


                string id = ExtractId(response);

                Console.WriteLine(id);


                string endpointTurnON = "http://" + textBoxIp.Text + "/rest/interface/wireless/*" + id + "";
                client.UploadString(endpointTurnON, "PATCH", jsonContent);


            }
        }

        private void buttonDesligarRedeWireless_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string jsonContent = @"
            {
                ""disabled"": ""true""
            }";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }
            Console.WriteLine("All checked items:");


            foreach (var item in checkedValues)
            {

                string[] parts = item.Split(new string[] { "->" }, StringSplitOptions.None);

                string nome = parts[0];



                string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireless?.proplist=.id&name=" + nome + "";
                string response = client.DownloadString(endpointGetId);


                string id = ExtractId(response);

                Console.WriteLine(id);


                string endpointTurnON = "http://" + textBoxIp.Text + "/rest/interface/wireless/*" + id + "";
                client.UploadString(endpointTurnON, "PATCH", jsonContent);


            }
        }
        private void buttonRefreshWifi_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetWifis = "http://" + textBoxIp.Text + "/rest/interface/wireless";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string endpointGetPerfil = "http://" + textBoxIp.Text + "/rest/interface/wireless/security-profiles";
            WebClient clientPerfil = new WebClient();
            clientPerfil.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            comboBoxWifi.Items.Clear();
            comboBoxPerfisWifi.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetWifis);
                var obj = JsonConvert.DeserializeObject(response);

                List<RedeWireless> redeWirelesses = JsonConvert.DeserializeObject<List<RedeWireless>>(obj.ToString());

                foreach (var iface in redeWirelesses)
                {

                    comboBoxWifi.Items.Add("" + iface.nome + "");

                }

                string responsePerfil = client.DownloadString(endpointGetPerfil);
                var objPerfil = JsonConvert.DeserializeObject(responsePerfil);
                Console.WriteLine(objPerfil);

                List<PerfilSeguranca> perfis = JsonConvert.DeserializeObject<List<PerfilSeguranca>>(objPerfil.ToString());

                foreach (var iface in perfis)
                {
                    comboBoxPerfisWifi.Items.Add(iface.name);

                }


            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();

            }
        }
        private void buttonGETWifi_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string wifiAEditar = comboBoxWifi.Text;

            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireless?.proplist=.id&name=" + wifiAEditar + "";


            string responseID = client.DownloadString(endpointGetId);
            var obj = JsonConvert.DeserializeObject(responseID);

            List<Identificador> ipId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[ipId.Count];
            for (int i = 0; i < ipId.Count; i++)
            {
                ids[i] = ipId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointWifi = "http://" + textBoxIp.Text + "/rest/interface/wireless/" + id + "";
                string responseWIFI = client.DownloadString(endpointWifi);

                RedeWireless wifi = JsonConvert.DeserializeObject<RedeWireless>(responseWIFI);

                textBoxNomeWlan.Text = wifi.nome;
                textBoxSSIDwifi.Text = wifi.ssid;
                textBoxMTUwifi.Text = wifi.mtu;
                comboBoxPerfisWifi.Text = wifi.securityProfile;

            }


        }

        private void buttonEditarWifi_Click(object sender, EventArgs e)
        {
            string wifiAEditar = comboBoxWifi.Text;

            string novoNomeWlan = textBoxNomeWlan.Text;
            string novoSSID = textBoxSSIDwifi.Text;
            string novoMTU = textBoxMTUwifi.Text;
            string novoPerfil = comboBoxPerfisWifi.Text;



            string credentials = GetCredentials();
            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireless?.proplist=.id&name=" + wifiAEditar + "";

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentEditWifi = @"{
                ""name"": """ + novoNomeWlan + @""",
                ""ssid"": """ + novoSSID + @""",
                ""mtu"": """ + novoMTU + @""",
                ""security-profile"": """ + novoPerfil + @"""
                
                 
            }";


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                //Console.WriteLine(obj);

                List<Identificador> wifiIds = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[wifiIds.Count];
                for (int i = 0; i < wifiIds.Count; i++)
                {
                    ids[i] = wifiIds[i].Id;
                }

                foreach (string id in ids)
                {
                    string endpointEditWifi = "http://" + textBoxIp.Text + "/rest/interface/wireless/" + id + "";
                    client.UploadString(endpointEditWifi, "PATCH", jsonContentEditWifi);
                }
                MessageBox.Show("REDE WIFI EDITADA COM SUCESSO");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO EDITAR REDE WIFI !");
            }
            finally
            {
                client.Dispose();

                comboBoxWifi.Items.Clear();
                comboBoxWifi.Text = "";
                textBoxNomeWlan.Clear();
                textBoxSSIDwifi.Clear();
                textBoxMTUwifi.Clear();
                comboBoxPerfisWifi.Items.Clear();
                comboBoxPerfisWifi.Text = "";
                

            }
        }


        #endregion



        #region DNS
        //####################---DNS---##################################################################################################################################################################################

        private void buttonCriarDNS_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string endpointCreateDNS = "http://" + textBoxIp.Text + "/rest/ip/dns/static/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";


            string jsonContent = @"{
                ""name"": """ + textBoxNomeDNS.Text + @""",
                ""address"": """ + textBoxIPDNS.Text + @""",
                ""ttl"": ""1d""
                 
            }";


            try
            {
                string response = client.UploadString(endpointCreateDNS, "POST", jsonContent);
                Console.WriteLine("Response: " + response);
                MessageBox.Show("DNS ESTATICO CRIADO");
            }
            catch (WebException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO CRIAR DNS ESTATICO");
            }
            finally
            {
                client.Dispose();
                
            }

        }

        private void buttonLigarRemoteRequests_Click(object sender, EventArgs e)
        {


            string credentials = GetCredentials();

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentLigarDNS = @"{

                ""allow-remote-requests"": ""true""
                 
            }";


            try
            {

                string endpointDNS = "http://" + textBoxIp.Text + "/rest/ip/dns/set";
                client.UploadString(endpointDNS, "POST", jsonContentLigarDNS);
                MessageBox.Show("ALLOW REMOTE REQUESTS: ON");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                client.Dispose();
            }
        }
        private void buttonDesligarRemoteRequests_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentLigarDNS = @"{

                ""allow-remote-requests"": ""false""
                 
            }";


            try
            {

                string endpointDNS = "http://" + textBoxIp.Text + "/rest/ip/dns/set";
                client.UploadString(endpointDNS, "POST", jsonContentLigarDNS);
                MessageBox.Show("ALLOW REMOTE REQUESTS: OFF ");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);

            }
            finally
            {
                client.Dispose();
            }
        }
        private void buttonListarEntradasEstaticasDNS_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetDNSStatic = "http://" + textBoxIp.Text + "/rest/ip/dns/static";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            checkedListBox7.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetDNSStatic);
                var obj = JsonConvert.DeserializeObject(response);
                //Console.WriteLine(obj
                List<ServidorDNS> DNSs = JsonConvert.DeserializeObject<List<ServidorDNS>>(obj.ToString());

                foreach (var iface in DNSs)
                {

                    checkedListBox7.Items.Add("" + iface.nome + " -> " + iface.address + "");

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();
            }
        }

        private void buttonEliminarEntradaEstaticaDNS_Click(object sender, EventArgs e)
        {

            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            List<string> checkedValues = new List<string>();
            foreach (var item in checkedListBox7.CheckedItems)
            {
                checkedValues.Add(item.ToString());
            }


            if (checkedListBox7.CheckedItems.Count == 0)
            {
                MessageBox.Show("SELECIONE UMA ENTRADA DNS PARA ELIMINAR!");
            }
            else
            {
                foreach (var item in checkedValues)
                {
                    try
                    {
                        string[] parts = item.Split(new string[] { "->" }, StringSplitOptions.None);

                        string nome = parts[0];


                        string endpointDeleteDNS = "http://" + textBoxIp.Text + "/rest/ip/dns/static/" + nome + "";
                        client.UploadString(endpointDeleteDNS, "DELETE", string.Empty);
                        MessageBox.Show("ENTRADA ESTATICA ELIMINADA!");
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("ERRO AO ELIMINAR ENTRADA ESTÁTICA DNS !");
                    }
                    finally
                    {
                        client.Dispose();
                        
                    }




                }
            }
        }


        #endregion

        #region WIREGUARD

        private void buttonRefreshPeerWG_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetWireguard = "http://" + textBoxIp.Text + "/rest/interface?type=wg";
            string enpointGetPeers = "http://" + textBoxIp.Text + "/rest/interface/wireguard/peers";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;



            comboBoxWG.Items.Clear();

            try
            {
                string response = client.DownloadString(enpointGetWireguard);
                var obj = JsonConvert.DeserializeObject(response);
                List<Wireguard> wireguards = JsonConvert.DeserializeObject<List<Wireguard>>(obj.ToString());



                foreach (var iface in wireguards)
                {

                    comboBoxWG.Items.Add("" + iface.nome + "");

                }



            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();

            }
        }

        private void buttonCriarPeer_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();

            string interfaceWireguard = comboBoxWG.Text;
            string addressWireguard = textBoxAllowAddPeer.Text;
            string publicKey = textBoxPublicKeyPeer.Text;


            string endpointCreatePeer = "http://" + textBoxIp.Text + "/rest/interface/wireguard/peers/add";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";


            if (!string.IsNullOrEmpty(textBoxAllowAddPeer.Text) && !string.IsNullOrEmpty(textBoxPublicKeyPeer.Text) && comboBoxWG.SelectedItem != null)
            {
                try
                {
                    string jsonContentPeer = @"{
                    ""interface"": """ + interfaceWireguard + @""",
                    ""public-key"": """ + publicKey + @""",
                    ""allowed-address"": """ + addressWireguard + @"""
                }";

                    string responsePeer = client.UploadString(endpointCreatePeer, "POST", jsonContentPeer);
                    Console.WriteLine("Response: " + responsePeer);
                    MessageBox.Show("PEER CRIADO !");
                }
                catch (WebException ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    client.Dispose();

                    comboBoxWG.Text = "";
                    textBoxAllowAddPeer.Text = "";
                    textBoxPublicKeyPeer.Text = "";
                }
            }
            else
            {

                MessageBox.Show("PREENCHA OS CAMPOS  !");
            }





        }

        private void buttonGetTunnel_Click(object sender, EventArgs e)
        {

            if (comboBoxPeerTunnel.SelectedItem != null && comboBoxWGTunel.SelectedItem != null)
            {
                string credentials = GetCredentials();
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                client.Headers[HttpRequestHeader.ContentType] = "application/json";

                string wireguardInterface = comboBoxWGTunel.Text;
                string peer = comboBoxPeerTunnel.Text;

                string endpointGetWireguard = "http://" + textBoxIp.Text + "/rest/interface/wireguard?name=" + wireguardInterface + "";
                string endpointGetPeer = "http://" + textBoxIp.Text + "/rest/interface/wireguard/peers?interface=" + peer + "";


                string responseWireguard = client.DownloadString(endpointGetWireguard);
                List<Wireguard> wireguards = JsonConvert.DeserializeObject<List<Wireguard>>(responseWireguard);

                string responsePeer = client.DownloadString(endpointGetPeer);
                List<Peer> peers = JsonConvert.DeserializeObject<List<Peer>>(responsePeer);

                string publicKey = "";
                string listenPort = "";

                foreach (var iface in wireguards)
                {

                    publicKey = iface.publicKey;
                    listenPort = iface.port;

                }

                string address = "";
                foreach (var iface in peers)
                {

                    address = iface.allowedAddress;


                }

                string wireguardConfig = @"
Address = " + address + @"
DNS = 8.8.8.8

[Peer]
PublicKey = " + publicKey + @"
AllowedIPs = 0.0.0.0/0
Endpoint = 192.168.88.1:" + listenPort + @"
PersistentKeepalive = 10";


                richTextBoxTunnelConfig.Text = wireguardConfig;
            }
            else
            {
                MessageBox.Show("SELECIONE OS CAMPOS  !");
            }
        }

        private void buttonRefreshWGPeer_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetWireguard = "http://" + textBoxIp.Text + "/rest/interface?type=wg";
            string enpointGetPeers = "http://" + textBoxIp.Text + "/rest/interface/wireguard/peers";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;




            comboBoxWGTunel.Items.Clear();
            comboBoxPeerTunnel.Items.Clear();
            try
            {
                string response = client.DownloadString(enpointGetWireguard);
                var obj = JsonConvert.DeserializeObject(response);
                List<Wireguard> wireguards = JsonConvert.DeserializeObject<List<Wireguard>>(obj.ToString());


                string responsePeer = client.DownloadString(enpointGetPeers);
                var objPeer = JsonConvert.DeserializeObject(responsePeer);
                List<Peer> peers = JsonConvert.DeserializeObject<List<Peer>>(objPeer.ToString());

                foreach (var iface in wireguards)
                {


                    comboBoxWGTunel.Items.Add("" + iface.nome + "");

                }

                foreach (var peer in peers)
                {

                    comboBoxPeerTunnel.Items.Add("" + peer.interfaceActual + "");

                }

            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();

            }
        }

        private void buttonCriarInterfaceWireguard_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetWireguard = "http://" + textBoxIp.Text + "/rest/interface?type=wg";
            string endpointCriarInterfaceWireguard = "http://" + textBoxIp.Text + "/rest/interface/wireguard/add";

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string responseWireguard = client.DownloadString(enpointGetWireguard);
          

            if (responseWireguard == "[]")
            {
                try
                {
                    string jsonContentPeer = @"{
                    ""name"": ""wireguardVPN""
                    
                }";

                    string responsePeer = client.UploadString(endpointCriarInterfaceWireguard, "POST", jsonContentPeer);
                    Console.WriteLine("Response: " + responsePeer);
                    MessageBox.Show("INTERFACE WIREGUARD CRIADA !");
                }
                catch (WebException ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                    MessageBox.Show("ERRO AO CRIAR INTERFACE WIREGUARD !");
                }
                finally
                {
                    client.Dispose();

                }
            }
            else
            {
                MessageBox.Show("JÁ EXISTE UMA INTERFACE WIREGUARD !");
            }


        }
        private void buttonInterfaceWireguard_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();
            string enpointGetWireguard = "http://" + textBoxIp.Text + "/rest/interface?type=wg";
            string enpointGetPeers = "http://" + textBoxIp.Text + "/rest/interface/wireguard/peers";
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;



            comboBoxInterfaceWireguard.Items.Clear();

            try
            {
                string response = client.DownloadString(enpointGetWireguard);
                var obj = JsonConvert.DeserializeObject(response);
                List<Wireguard> wireguards = JsonConvert.DeserializeObject<List<Wireguard>>(obj.ToString());



                foreach (var iface in wireguards)
                {

                    comboBoxInterfaceWireguard.Items.Add("" + iface.nome + "");

                }



            }

            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                client.Dispose();

            }
        }

        private void buttonGETPortoWG_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string interfaceWG = comboBoxInterfaceWireguard.Text;

            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireguard?name=" + interfaceWG + "";


            string responseID = client.DownloadString(endpointGetId);
         

            List<Identificador> ipId = JsonConvert.DeserializeObject<List<Identificador>>(responseID);

            string[] ids = new string[ipId.Count];
            for (int i = 0; i < ipId.Count; i++)
            {
                ids[i] = ipId[i].Id;
            }

            foreach (string id in ids)
            {
                string endpointWG = "http://" + textBoxIp.Text + "/rest/interface/wireguard/" + id + "";
                string responseWG = client.DownloadString(endpointWG);

                Wireguard wireguard = JsonConvert.DeserializeObject<Wireguard>(responseWG);

                textBoxPortoWG.Text = wireguard.port;
       

            }
        }

        private void buttonEditarPorto_Click(object sender, EventArgs e)
        {
            string wgaEditar = comboBoxInterfaceWireguard.Text;

            string porto = textBoxPortoWG.Text;


            string credentials = GetCredentials();
            string endpointGetId = "http://" + textBoxIp.Text + "/rest/interface/wireguard?name=" + wgaEditar + "";

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;

            string jsonContentEditWG = @"{
                ""listen-port"": """ + porto + @"""

            }";


            try
            {
                string response = client.DownloadString(endpointGetId);
                var obj = JsonConvert.DeserializeObject(response);
                //Console.WriteLine(obj);

                List<Identificador> wgID = JsonConvert.DeserializeObject<List<Identificador>>(response);

                string[] ids = new string[wgID.Count];
                for (int i = 0; i < wgID.Count; i++)
                {
                    ids[i] = wgID[i].Id;
                }

                foreach (string id in ids)
                {
                    string endpointWG = "http://" + textBoxIp.Text + "/rest/interface/wireguard/" + id + "";
                    client.UploadString(endpointWG, "PATCH", jsonContentEditWG);
                }
                MessageBox.Show("PORTO EDITADO COM SUCESSO");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("ERRO AO EDITAR PORTO !");
            }
            finally
            {
                client.Dispose();

                textBoxPortoWG.Text = "";
                comboBoxInterfaceWireguard.Text = "";


            }


        }

        private void buttonLigarWG_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string jsonContent = @"
            {
                ""disabled"": ""false""
            }";



            string endpointGetID = "http://" + textBoxIp.Text + "/rest/interface/wireguard";
            string response = client.DownloadString(endpointGetID);


            List<Identificador> wgID = JsonConvert.DeserializeObject<List<Identificador>>(response);

            if (response != "[]")
            {
                string[] ids = new string[wgID.Count];
                for (int i = 0; i < wgID.Count; i++)
                {
                    ids[i] = wgID[i].Id;
                }

                foreach (string id in ids)
                {

                    string endpointGetDisabled = "http://" + textBoxIp.Text + "/rest/interface/wireguard";
                    string responseStatus = client.DownloadString(endpointGetDisabled);
                    List<Wireguard> wgStatus = JsonConvert.DeserializeObject<List<Wireguard>>(responseStatus);

                    if (wgStatus[0].disabled == "true")
                    {
                        string endpointTurnON = "http://" + textBoxIp.Text + "/rest/interface/wireguard/" + id + "";
                        client.UploadString(endpointTurnON, "PATCH", jsonContent);
                        MessageBox.Show("INTERFACE LIGADA !");
                    }
                    else if (wgStatus[0].disabled == "false")
                    {
                        MessageBox.Show("A INTERFACE JÁ SE ENCONTRA LIGADA !");

                    }


                }


            }
            else
            {
                MessageBox.Show("NÃO EXISTE INTERFACE WIREGUARD !");
            }

           

        }

        private void buttonDesligarWG_Click(object sender, EventArgs e)
        {
            string credentials = GetCredentials();


            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string jsonContent = @"
            {
                ""disabled"": ""true""
            }";



            string endpointGetID = "http://" + textBoxIp.Text + "/rest/interface/wireguard";
            string response = client.DownloadString(endpointGetID);


            List<Identificador> wgID = JsonConvert.DeserializeObject<List<Identificador>>(response);


            if (response != "[]")
            {
                string[] ids = new string[wgID.Count];
            for (int i = 0; i < wgID.Count; i++)
            {
                ids[i] = wgID[i].Id;
            }

            foreach (string id in ids)
            {

                string endpointGetDisabled = "http://" + textBoxIp.Text + "/rest/interface/wireguard";
                string responseStatus = client.DownloadString(endpointGetDisabled);
                List<Wireguard> wgStatus = JsonConvert.DeserializeObject<List<Wireguard>>(responseStatus);

                if (wgStatus[0].disabled == "false")
                {
                    string endpointTurnON = "http://" + textBoxIp.Text + "/rest/interface/wireguard/" + id + "";
                    client.UploadString(endpointTurnON, "PATCH", jsonContent);

                    MessageBox.Show("INTERFACE DESLIGADA !");
                }
                else if (wgStatus[0].disabled == "true")
                {
                    MessageBox.Show("A INTERFACE JÁ SE ENCONTRA DESLIGADA !");

                }


            }
            }
            else
            {
                MessageBox.Show("NÃO EXISTE INTERFACE WIREGUARD !");
            }
        }

        #endregion


        #region TEXTBOX

        private void textBoxNomeBridge_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(textBoxNomeBridge.Text))
            {
                // Enable the button if the desired value is selected
                buttonCriarBridge.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonCriarBridge.Enabled = false;

            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            /* if (checkedListBox2.Items.Count > 0)
             {
                 // Enable the button if at least one item is checked
                 buttonEliminarBridge.Enabled = true;
             }
             else
             {
                 // Disable the button if no item is checked
                 buttonEliminarBridge.Enabled = false;
             }*/



        }

        private void textBoxNomePerfil_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxNomePerfil.Text))
            {
                // Enable the button if the desired value is selected
                buttonCriarPerfil.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonCriarPerfil.Enabled = false;

            }
        }

        private void textBoxEnderecoRota_TextChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(textBoxEnderecoRota.Text))
            {
                // Enable the button if the desired value is selected
                buttonCriarRotaEstatica.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonCriarRotaEstatica.Enabled = false;

            }
        }

        private void textBoxEnderecoIP_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxEnderecoIP.Text))
            {
                // Enable the button if the desired value is selected
                buttonCriarEnderecoIP.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonCriarEnderecoIP.Enabled = false;

            }
        }

        private void textBoxNameDHCP_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxNameDHCP.Text))
            {
                // Enable the button if the desired value is selected
                buttonCriarServidorDHCP.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonCriarServidorDHCP.Enabled = false;

            }
        }

        private void DNS_Click(object sender, EventArgs e)
        {

        }

        private void textBoxNomeDNS_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxNomeDNS.Text))
            {
                // Enable the button if the desired value is selected
                buttonCriarDNS.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonCriarDNS.Enabled = false;

            }
        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox3.CheckedItems.Count > 0)
            {
                // Enable the button if at least one item is checked
                buttonEliminarPerfil.Enabled = true;
            }
            else
            {
                // Disable the button if no item is checked
                buttonEliminarPerfil.Enabled = false;
            }
        }

        private void checkedListBox4_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        private void checkedListBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox5.CheckedItems.Count > 0)
            {
                // Enable the button if at least one item is checked
                buttonEliminarEndereçoIP.Enabled = true;
            }
            else
            {
                // Disable the button if no item is checked
                buttonEliminarEndereçoIP.Enabled = false;
            }
        }

        /*private void checkedListBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox2.CheckedItems.Count > 0)
            {
                // Enable the button if at least one item is checked
               buttonEliminarDHCP.Enabled = true;
            }
            else
            {
                // Disable the button if no item is checked
                buttonEliminarDHCP.Enabled = false;
            }
        }*/

        /*private void checkedListBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBox7.CheckedItems.Count > 0)
            {
                // Enable the button if at least one item is checked
                buttonEliminarEntradaEstaticaDNS.Enabled = true;
               
            }
            else
            {
                // Disable the button if no item is checked
                buttonEliminarEntradaEstaticaDNS.Enabled = false;
            }
        }*/

        private void comboBoxPerfis_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxPerfis.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGETPerfil.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGETPerfil.Enabled = false;

            }
        }

        private void comboBoxRotas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRotas.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGETRotas.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGETRotas.Enabled = false;

            }
        }

        private void comboBoxIPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxIPs.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGETIP.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGETIP.Enabled = false;

            }
        }



        private void comboBoxDHCPs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxDHCPs.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGETDHCP.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGETDHCP.Enabled = false;

            }
        }

        private void comboBoxWifi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxWifi.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGETWifi.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGETWifi.Enabled = false;

            }
        }



        private void comboBoxWGTunel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxWGTunel.SelectedItem != null)
            {
                // Enable the button if the desired value is selected
                buttonGetTunnel.Enabled = true;
            }
            else
            {
                // Disable the button if the desired value is not selected
                buttonGetTunnel.Enabled = false;

            }
        }

        private void textBoxPasswordPerfil_TextChanged(object sender, EventArgs e)
        {

        }

        
    }

    #endregion

}
