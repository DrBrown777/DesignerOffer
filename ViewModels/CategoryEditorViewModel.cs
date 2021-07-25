using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace Designer_Offer.ViewModels
{
    internal class CategoryEditorViewModel : ViewModel
    {
        private string _Name;
        /// <summary>
        /// Название категории
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private List<Section> _Sections;
        /// <summary>
        /// Разделы
        /// </summary>
        public List<Section> Sections
        {
            get => _Sections;
            set => Set(ref _Sections, value);
        }

        private Section _SelectedSection;
        /// <summary>
        /// Выбранный раздел
        /// </summary>
        public Section SelectedSection
        {
            get => _SelectedSection;
            set => Set(ref _SelectedSection, value);
        }

        public CategoryEditorViewModel(IRepository<Section> repaSection)
        {
            Sections = repaSection.Items.ToList();
        }
    }
}
