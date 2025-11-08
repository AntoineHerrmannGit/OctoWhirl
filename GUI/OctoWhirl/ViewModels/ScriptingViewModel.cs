using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using OctoWhirl.GUI.Commands;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OctoWhirl.GUI.ViewModels
{
    public class ScriptingViewModel : BaseViewModel
    {
        private readonly string _scriptsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");

        public ObservableCollection<ScriptItem> Scripts { get; } = new();
        public ObservableCollection<ScriptItem> OpenedScripts { get; } = new();

        private ScriptItem _selectedScript;
        public ScriptItem SelectedScript
        {
            get => _selectedScript;
            set
            {
                if (SetProperty(ref _selectedScript, value) && value != null)
                {
                    OpenScript(value);
                }
            }
        }

        private ScriptItem _activeScript;
        public ScriptItem ActiveScript
        {
            get => _activeScript;
            set => SetProperty(ref _activeScript, value);
        }

        public ICommand CompileCommand { get; }
        public ICommand RunCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ReloadScriptsCommand { get; }

        public ScriptingViewModel()
        {
            CompileCommand = new RelayCommand(CompileAll);
            RunCommand = new RelayCommand(async () => await Run());
            SaveCommand = new RelayCommand(SaveActiveScript);
            ReloadScriptsCommand = new RelayCommand(LoadScripts);

            LoadScripts();
        }

        /// <summary>
        /// Charge tous les scripts présents dans le répertoire.
        /// </summary>
        private void LoadScripts()
        {
            Scripts.Clear();

            if (!Directory.Exists(_scriptsDirectory))
                Directory.CreateDirectory(_scriptsDirectory);

            foreach (var file in Directory.GetFiles(_scriptsDirectory, "*.csx"))
            {
                Scripts.Add(new ScriptItem
                {
                    Name = Path.GetFileName(file),
                    Path = file,
                    Code = File.ReadAllText(file)
                });
            }
        }

        /// <summary>
        /// Ouvre un script dans un onglet.
        /// </summary>
        private void OpenScript(ScriptItem script)
        {
            if (!OpenedScripts.Contains(script))
                OpenedScripts.Add(script);

            ActiveScript = script;
        }

        /// <summary>
        /// Sauvegarde le script actif sur disque.
        /// </summary>
        private void SaveActiveScript()
        {
            if (ActiveScript == null) return;

            try
            {
                File.WriteAllText(ActiveScript.Path, ActiveScript.Code);
                MessageBox.Show($"Script {ActiveScript.Name} sauvegardé.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur de sauvegarde : " + ex.Message);
            }
        }

        /// <summary>
        /// Compile tous les scripts ouverts.
        /// </summary>
        private void CompileAll()
        {
            foreach (var script in OpenedScripts)
            {
                try
                {
                    var csharpScript = CSharpScript.Create(script.Code, ScriptOptions.Default);
                    var diagnostics = csharpScript.Compile();

                    if (diagnostics.Any(d => d.Severity == Microsoft.CodeAnalysis.DiagnosticSeverity.Error))
                    {
                        MessageBox.Show($"Erreur de compilation dans {script.Name}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur dans {script.Name} : {ex.Message}");
                }
            }

            MessageBox.Show("Compilation terminée.");
        }

        /// <summary>
        /// Exécute le script actif.
        /// </summary>
        private async Task Run()
        {
            if (ActiveScript == null) return;

            try
            {
                var result = await CSharpScript.EvaluateAsync(
                    ActiveScript.Code,
                    ScriptOptions.Default);

                MessageBox.Show($"Résultat ({ActiveScript.Name}) : {result ?? "null"}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur d'exécution : " + ex.Message);
            }
        }
    }

    public class ScriptItem : BaseViewModel
    {
        public string Name { get; set; }
        public string Path { get; set; }

        private string _code;
        public string Code
        {
            get => _code;
            set => SetProperty(ref _code, value);
        }
    }
}
