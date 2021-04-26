﻿using Designer_Offer.Data;
using Designer_Offer.Infrastructure.Commands;
using Designer_Offer.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace Designer_Offer.ViewModels
{
    internal class LoginViewModel : ViewModel
    {
        private List<Company> _Companies;
        /// <summary>
        /// Список компаний
        /// </summary>
        public List<Company> Companies
        {
            get => _Companies;
            set => Set(ref _Companies, value);
        }

        private string _Login;
        /// <summary>
        /// Поле Логин
        /// </summary>
        public string Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }

        private string _SelectedComapany;
        /// <summary>
        /// Выбранная компания
        /// </summary>
        public string SelectedCompany
        {
            get => _SelectedComapany;
            set => Set(ref _SelectedComapany, value);
        }
        /// <summary>
        /// Команда загрузки страницы Регистрации
        /// </summary>
        public ICommand LoadRegistarationPageCommand { get; }

        public ICommand LoginCommand { get; }
        /// <summary>
        /// Команда Входим в систему
        /// </summary>
        private void OnLoginCommand(object p)
        {
            int id = Convert.ToInt32(SelectedCompany);
            PasswordBox passBox = (PasswordBox)p;
            string pass = passBox.Password;

        }

        private bool CanLoginCommand(object p)
        {
            return true && SelectedCompany != null; 
        }

        public LoginViewModel(ICommand loadregister)
        {
            if (contextDB != null) 
                Companies = contextDB.Company.ToList();

            LoginCommand = new LambdaCommand(OnLoginCommand, CanLoginCommand);
            LoadRegistarationPageCommand = loadregister;

            Status = "Для входа в систему введите Логин и Пароль";
            Title = "Designer Offer :: Вход в систему";
        }
    }
}
