using Ookii.Dialogs.Wpf;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace CodeGenTreeView
{
    public class TreeViewItemData : INotifyPropertyChanged
    {
        private string name;
        private string icon;
        private ObservableCollection<TreeViewItemData> children;

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Icon
        {
            get { return icon; }
            set
            {
                if (icon != value)
                {
                    icon = value;
                    OnPropertyChanged(nameof(Icon));
                }
            }
        }

        public ObservableCollection<TreeViewItemData> Children
        {
            get { return children; }
            set
            {
                if (children != value)
                {
                    children = value;
                    OnPropertyChanged(nameof(Children));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            var folderDialog = new VistaFolderBrowserDialog();

            if (folderDialog.ShowDialog(this) == true)
            {
                selectedFolderText.Text = $"Selected Folder: {folderDialog.SelectedPath}";
                DisplayFolderContents(folderDialog.SelectedPath);
            }
        }

        private void DisplayFolderContents(string path)
        {
            folderTreeView.Items.Clear();

            var rootNode = new TreeViewItemData
            {
                Name = path,
                Icon = "/CodeGenTreeView;component/Resources/folder-api.png",
                Children = new ObservableCollection<TreeViewItemData>()
            };
            folderTreeView.Items.Add(rootNode);

            AddDirectoriesAndFiles(new DirectoryInfo(path), rootNode);
        }

        private void AddDirectoriesAndFiles(DirectoryInfo directory, TreeViewItemData parentNode)
        {
            foreach (var subDirectory in directory.GetDirectories())
            {
                var subNode = new TreeViewItemData
                {
                    Name = subDirectory.Name,
                    Icon = "/CodeGenTreeView;component/Resources/folder-api.png",
                    Children = new ObservableCollection<TreeViewItemData>()
                };

                parentNode.Children.Add(subNode);
                AddDirectoriesAndFiles(subDirectory, subNode);
            }

            foreach (var file in directory.GetFiles())
            {
                var fileNode = new TreeViewItemData
                {
                    Name = file.Name,
                    Icon = "/CodeGenTreeView;component/Resources/csharp.png"
                };

                parentNode.Children.Add(fileNode);
            }
        }
    }
}