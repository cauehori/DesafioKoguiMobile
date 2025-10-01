using KoguiApp.Models;
using KoguiApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; // Para ObservableCollection
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KoguiApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        // Para controlar a aba selecionada
        private int _selectedSection = 1; // Começa na seção 1
        public int SelectedSection
        {
            get => _selectedSection;
            set
            {
                _selectedSection = value;
                OnPropertyChanged(); // Notifica a tela que o valor mudou
            }
        }
        public ICommand SetSectionCommand { get; }

        public ObservableCollection<ChaveCor> ItensChaveCor { get; set; }

        public MainViewModel()
        {
            // Instancia a lista com os valores
            ItensChaveCor = new ObservableCollection<ChaveCor>
            {
               
                // String vazia pois não foi fornecida.
                new ChaveCor { HEX = "", COR = "Magenta Fuchsia", COMPONENTE = "" }, 
                new ChaveCor { HEX = "", COR = "White", COMPONENTE = "para" }, 
                new ChaveCor { HEX = "", COR = "Blue", COMPONENTE = "Pares" }, 
                new ChaveCor { HEX = "", COR = "Green", COMPONENTE = "alterar" }, 
                new ChaveCor { HEX = "", COR = "Black", COMPONENTE = "#" }, 
                new ChaveCor { HEX = "", COR = "Web Orange", COMPONENTE = "e" }, 
                new ChaveCor { HEX = "", COR = "Yellow", COMPONENTE = "impares" }, 
                new ChaveCor { HEX = "", COR = "Red", COMPONENTE = "\" \"" }, 
                new ChaveCor { HEX = "", COR = "Coconut", COMPONENTE = "Busca" }, 
                new ChaveCor { HEX = "", COR = "CyanAqua", COMPONENTE = "primos" } 
            };

            // Comando para os botões
            SetSectionCommand = new Command<string>((section) =>
            {
                SelectedSection = int.Parse(section);
            });

            // Para chamar o método e carregar os dados da seção 2
            ProcessarCoresApiAsync();

            // Para chamar o método e carregar os dados da seção 3
            ProcessarMatrizAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Propriedades para a Seção 2
        public ObservableCollection<ApiColorResult> ApiResults { get; set; }

        private string _fraseResultante;
        public string FraseResultante
        {
            get => _fraseResultante;
            set
            {
                _fraseResultante = value;
                OnPropertyChanged(); // AVISO IMPORTANTE PARA A TELA!
            }
        }
        private async Task ProcessarCoresApiAsync()
        {
            var colorApiService = new ColorApiService(); 
            var hexValues = new List<string> { "#0000FF", "#00FF00", "#FFFFFF", "#FF0000", "#FFA500", "#FFFF00", "#000000" }; 

            var fraseParts = new List<string>();
            ApiResults = new ObservableCollection<ApiColorResult>();

            foreach (var hex in hexValues)
            {
                var colorName = await colorApiService.GetColorNameAsync(hex); // Chama a API 

                // Adiciona resultado para exibição nos cards
                ApiResults.Add(new ApiColorResult { NomeCor = colorName, Hexadecimal = hex }); 

                // Busca o componente correspondente na lista inicial 
                var itemCorrespondente = ItensChaveCor.FirstOrDefault(item => item.COR.Contains(colorName));
                if (itemCorrespondente != null)
                {
                    fraseParts.Add(itemCorrespondente.COMPONENTE);
                }
            }

            FraseResultante = string.Join(" ", fraseParts); // Monta a frase final 
                                                           
        }

        // Modelo auxiliar para a lista da Seção 2
        public class ApiColorResult
        {
            public string NomeCor { get; set; }
            public string Hexadecimal { get; set; }
        }

        // Propriedades para a Seção 3
        private string _figuraResultante;
        public string FiguraResultante
        {
            get => _figuraResultante;
            set
            {
                _figuraResultante = value;
                OnPropertyChanged();
            }
        }

        private string _nomeObjeto;
        public string NomeObjeto
        {
            get => _nomeObjeto;
            set
            {
                _nomeObjeto = value;
                OnPropertyChanged();
            }
        }
        private async Task ProcessarMatrizAsync()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            
            var resourceName = "KoguiApp.MATRIZ.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string matrizContent = await reader.ReadToEndAsync();
                var resultBuilder = new System.Text.StringBuilder();

                foreach (char c in matrizContent)
                {
                    if (char.IsDigit(c))
                    {
                        int digit = c - '0'; // Converte char para int
                        if (digit % 2 == 0) // É par?
                        {
                            resultBuilder.Append(" "); // Altera para espaço
                        }
                        else // É ímpar?
                        {
                            resultBuilder.Append("#"); // Altera para #
                        }
                    }
                }

                
                
                // A largura correta para esta matriz é 118 caracteres.
                int larguraLinha = 118;
                string asciiArtRaw = resultBuilder.ToString();
                var formattedArt = new System.Text.StringBuilder();
                for (int i = 0; i < asciiArtRaw.Length; i += larguraLinha)
                {
                    if (i + larguraLinha < asciiArtRaw.Length)
                    {
                        formattedArt.AppendLine(asciiArtRaw.Substring(i, larguraLinha));
                    }
                    else
                    {
                        formattedArt.Append(asciiArtRaw.Substring(i));
                    }
                }

                FiguraResultante = formattedArt.ToString();

               
                NomeObjeto = "Logo da Kogui";
            }
        }
    }
}
