using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Designer_Offer.Services
{
    internal class UserDialogService : IUserDialog
    {
        public bool Edit(object item, params object[] items)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            switch (item)
            {
                case Client client:
                    return EditClient(client);
                case Build build:
                    return EditBuild(build);
                case Company company:
                    return EditCompany(company, (List<Position>)items[0]);
                case Employee employee:
                    return EditEmploee(employee, (List<Company>)items[0]);
                case Position position:
                    return EditPosition(position);
                case Section section:
                    return EditSection(section);
                case Unit unit:
                    return EditUnit(unit);
                case Supplier supplier:
                    return EditSupplier(supplier);
                case Category category:
                    return EditCategory(category);
                default:
                    throw new NotSupportedException($"Редактирование обьекта типа {item.GetType().Name} не поддерживается.");
            }
        }

        private static bool EditClient(Client client)
        {
            var client_editor_window = App.Host.Services
                                        .GetRequiredService<ClientEditorWindow>();
            var client_editor_model = App.Host.Services
                                        .GetRequiredService<ClientEditorViewModel>();

            client_editor_model.Name = client.Name;

            client_editor_window.DataContext = client_editor_model;

            if (client_editor_window.ShowDialog() != true) return false;

            client.Name = client_editor_model.Name;

            return true;
        }

        private static bool EditBuild(Build build)
        {
            var build_editor_window = App.Host.Services
                                        .GetRequiredService<BuildEditorWindow>();
            var build_editor_model = App.Host.Services
                                        .GetRequiredService<BuildEditorViewModel>();

            build_editor_model.Name = build.Name;
            build_editor_model.Adress = build.Adress;
            build_editor_model.Clients = App.Host.Services
                                        .GetRequiredService<ProjectManagerViewModel>()
                                        .Clients;

            build_editor_model.Project = build.Project;
            build_editor_model.SelectedClient = build_editor_model.Clients
                                        .SingleOrDefault(c => c.Id == build.Client_Id);

            build_editor_window.DataContext = build_editor_model;

            if (build_editor_window.ShowDialog() != true) return false;

            build.Name = build_editor_model.Name;
            build.Adress = build_editor_model.Adress;
            build.Project = build_editor_model.Project;
            build.Client_Id = build_editor_model.SelectedClient.Id;

            return true;
        }

        private static bool EditCompany(Company company, List<Position> positions)
        {
            var company_editor_window = App.Host.Services
                                        .GetRequiredService<CompanyEditorWindow>();
            var company_editor_model = App.Host.Services
                                        .GetRequiredService<CompanyEditorViewModel>();

            company_editor_model.Name = company.Name;
            company_editor_model.Address = company.Adress;
            company_editor_model.Phone = company.Phone;
            company_editor_model.Email = company.Mail;
            company_editor_model.CompanyPosition = company.Position;

            company_editor_model.Position = positions;

            company_editor_window.DataContext = company_editor_model;

            if (company_editor_window.ShowDialog() != true) return false;

            company.Name = company_editor_model.Name;
            company.Adress = company_editor_model.Address;
            company.Phone = company_editor_model.Phone;
            company.Mail = company_editor_model.Email;
            company.Position = company_editor_model.CompanyPosition;

            return true;
        }

        private static bool EditEmploee(Employee employee, List<Company> companies)
        {
            var employee_editor_window = App.Host.Services
                                        .GetRequiredService<EmployeeEditorWindow>();
            var employee_editor_model = App.Host.Services
                                        .GetRequiredService<EmployeeEditorViewModel>();

            employee_editor_model.UserLogin = employee.UserData.Login;
            employee_editor_model.UserPassword = employee.UserData.Password;
            employee_editor_model.UserName = employee.First_Name;
            employee_editor_model.UserSurName = employee.Last_Name;
            employee_editor_model.UserEmail = employee.Mail;
            employee_editor_model.UserPhone = employee.Phone;
            employee_editor_model.SelectedCompany = employee.Company;
            employee_editor_model.SelectedPosition = employee.Position;

            employee_editor_model.Companies = companies;

            employee_editor_window.DataContext = employee_editor_model;

            if (employee_editor_window.ShowDialog() != true) return false;

            employee.UserData.Login = employee_editor_model.UserLogin;
            employee.UserData.Password = employee_editor_model.UserPassword;
            employee.First_Name = employee_editor_model.UserName;
            employee.Last_Name = employee_editor_model.UserSurName;
            employee.Mail = employee_editor_model.UserEmail;
            employee.Phone = employee_editor_model.UserPhone;
            employee.Company = employee_editor_model.SelectedCompany;
            employee.Position = employee_editor_model.SelectedPosition;

            return true;
        }

        private static bool EditPosition(Position position)
        {
            var position_editor_window = App.Host.Services
                                            .GetRequiredService<PositionEditorWindow>();

            var position_editor_model = App.Host.Services
                                            .GetRequiredService<PositionEditorViewModel>();

            position_editor_model.Name = position.Name;

            position_editor_window.DataContext = position_editor_model;

            if (position_editor_window.ShowDialog() != true) return false;

            position.Name = position_editor_model.Name;

            return true;
        }

        private static bool EditSection(Section section)
        {
            var section_editor_window = App.Host.Services
                                        .GetRequiredService<SectionEditorWindow>();
            var section_editor_model = App.Host.Services
                                        .GetRequiredService<SectionEditorViewModel>();

            section_editor_model.Name = section.Name;

            section_editor_window.DataContext = section_editor_model;

            if (section_editor_window.ShowDialog() != true) return false;

            section.Name = section_editor_model.Name;

            return true;
        }

        private static bool EditUnit(Unit unit)
        {
            var unit_editor_window = App.Host.Services
                                       .GetRequiredService<UnitEditorWindow>();
            var unit_editor_model = App.Host.Services
                                       .GetRequiredService<UnitEditorViewModel>();

            unit_editor_model.Name = unit.Name;

            unit_editor_window.DataContext = unit_editor_model;

            if (unit_editor_window.ShowDialog() != true) return false;

            unit.Name = unit_editor_model.Name;

            return true;
        }

        private static bool EditSupplier(Supplier supplier)
        {
            var supplier_editor_window = App.Host.Services
                                            .GetRequiredService<SupplierEditorWindow>();
            var supplier_editor_model = App.Host.Services
                                            .GetRequiredService<SupplierEditorViewModel>();

            supplier_editor_model.Name = supplier.Name;

            supplier_editor_window.DataContext = supplier_editor_model;

            if (supplier_editor_window.ShowDialog() != true) return false;

            supplier.Name = supplier_editor_model.Name;

            return true;
        }

        private static bool EditCategory(Category category)
        {
            var category_editor_window = App.Host.Services
                                       .GetRequiredService<CategoryEditorWindow>();
            var category_editor_model = App.Host.Services
                                        .GetRequiredService<CategoryEditorViewModel>();

            category_editor_model.Name = category.Name;
            category_editor_model.SelectedSection = category.Section;

            category_editor_window.DataContext = category_editor_model;

            if (category_editor_window.ShowDialog() != true) return false;

            category.Name = category_editor_model.Name;
            category.Section = category_editor_model.SelectedSection;

            return true;
        }

        public bool ConfirmInformation(string Information, string Caption)
        {
            return MessageBox.Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Information) == MessageBoxResult.Yes;
        }

        public bool ConfirmWarning(string Information, string Caption)
        {
            return MessageBox.Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes;
        }

        public bool ConfirmError(string Information, string Caption)
        {
            return MessageBox.Show(
                Information, Caption,
                MessageBoxButton.YesNo,
                MessageBoxImage.Error) == MessageBoxResult.Yes;
        }

        public void ShowError(string Information, string Caption)
        {
            MessageBox.Show(
                Information, Caption,
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public void ShowInformation(string Information, string Caption)
        {
            MessageBox.Show(
               Information, Caption,
               MessageBoxButton.OK,
               MessageBoxImage.Information);
        }
    }
}
