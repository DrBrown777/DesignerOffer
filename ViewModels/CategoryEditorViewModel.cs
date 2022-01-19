using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class CategoryEditorViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _Name;
        /// <summary>
        /// Название категории
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private ICollection<Sections> _CategorySections;
        /// <summary>
        /// Разделы выбранной категории
        /// </summary>
        public ICollection<Sections> CategorySections
        {
            get => _CategorySections;
            set => Set(ref _CategorySections, value);
        }

        private List<Sections> _Sections;
        /// <summary>
        /// Все разделы
        /// </summary>
        public List<Sections> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Добавление разделов в категорию
        /// </summary>
        public ICommand AddSections { get; }

        private bool CanAddSections(object p) => true && p != null;

        private void OnAddSections(object p)
        {
            ListBox listBox = (ListBox)p;

            CategorySections?.Clear();

            foreach (Sections item in listBox.SelectedItems)
            {
                CategorySections.Add(item);
            }
        }

        /// <summary>
        /// Автоматическая Выборка разделов у редактируемой категории
        /// </summary>
        public ICommand ChoiceSections { get; }

        private bool CanChoiceSections(object p) => true && p != null;

        private void OnChoiceSections(object p)
        {
            ListBox listBox = (ListBox)p;

            foreach (Sections item in listBox.Items)
            {
                if (CategorySections.Contains(item))
                {
                    listBox.SelectedItems.Add(item);
                }
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public CategoryEditorViewModel(IRepository<Sections> repaSection)
        {
            Sections = repaSection.Items.ToList();

            AddSections = new LambdaCommand(OnAddSections, CanAddSections);
            ChoiceSections = new LambdaCommand(OnChoiceSections, CanChoiceSections);
        }
        #endregion
    }
}
