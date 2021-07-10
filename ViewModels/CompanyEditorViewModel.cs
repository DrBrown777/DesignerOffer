﻿using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class CompanyEditorViewModel : ViewModel
    {
        #region СВОЙСТВА
        private string _Name;
        /// <summary>
        /// Название компании
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _Address;
        /// <summary>
        /// Адресс компании
        /// </summary>
        public string Address
        {
            get => _Address;
            set => Set(ref _Address, value);
        }

        private string _Phone;
        /// <summary>
        /// Телефон компании
        /// </summary>
        public string Phone
        {
            get => _Phone;
            set => Set(ref _Phone, value);
        }

        private string _Email;
        /// <summary>
        /// E-mail компании
        /// </summary>
        public string Email
        {
            get => _Email;
            set => Set(ref _Email, value);
        }

        private ICollection<Position> _CompanyPosition;
        /// <summary>
        /// Должности компании
        /// </summary>
        public ICollection<Position> CompanyPosition
        {
            get => _CompanyPosition;
            set => Set(ref _CompanyPosition, value);
        }

        private List<Position> _Position;
        /// <summary>
        /// Все должности
        /// </summary>
        public List<Position> Position
        {
            get => _Position;
            set => Set(ref _Position, value);
        }
        #endregion

        #region КОМАНДЫ
        /// <summary>
        /// Добавление позиций в компанию
        /// </summary>
        public ICommand AddPosition { get; }

        private bool CanAddPosition(object p) => true && p != null;

        private void OnAddPosition(object p)
        {
            ListBox listBox = (ListBox)p;

            CompanyPosition?.Clear();

            foreach (Position item in listBox.SelectedItems)
            {
                CompanyPosition.Add(item);
            }
        }
        /// <summary>
        /// Автоматическая Выборка должностей у редактируемой компании
        /// </summary>
        public ICommand ChoicePosition { get; }

        private bool CanChoicePosition(object p) => true && p != null;

        private void OnChoicePosition(object p)
        {
            ListBox listBox = (ListBox)p;

            foreach (Position item in listBox.Items)
            {
                if (CompanyPosition.Contains(item))
                {
                    listBox.SelectedItems.Add(item);
                }
            }
        }
        #endregion

        #region КОНСТРУКТОРЫ
        public CompanyEditorViewModel()
        {
            ChoicePosition = new LambdaCommand(OnChoicePosition, CanChoicePosition);
            AddPosition = new LambdaCommand(OnAddPosition, CanAddPosition);
            CompanyPosition = new List<Position>();
        }
        #endregion
    }
}