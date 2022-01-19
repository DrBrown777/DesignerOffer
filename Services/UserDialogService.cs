using Designer_Offer.Data;
using Designer_Offer.Services.Interfaces;
using Designer_Offer.ViewModels;
using Designer_Offer.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Windows;

namespace Designer_Offer.Services
{
    internal class UserDialogService : IUserDialog
    {
        public bool Edit(object item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            switch (item)
            {
                case Clients client:
                    return EditClient(client);
                case Builds build:
                    return EditBuild(build);
                case Companies company:
                    return EditCompany(company);
                case Employees employee:
                    return EditEmploee(employee);
                case Positions position:
                    return EditPosition(position);
                case Sections section:
                    return EditSection(section);
                case Units unit:
                    return EditUnit(unit);
                case Suppliers supplier:
                    return EditSupplier(supplier);
                case Categories category:
                    return EditCategory(category);
                case Products product:
                    return EditProduct(product);
                case Installs install:
                    return EditInstall(install);
                case Manufacturers manufacturer:
                    return EditManufacturer(manufacturer);
                case Offers offer:
                    return EditOffer(offer);
                default:
                    throw new NotSupportedException($"Редактирование обьекта типа {item.GetType().Name} не поддерживается.");
            }
        }

        private static bool EditClient(Clients client)
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

        private static bool EditBuild(Builds build)
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

            build_editor_model.Project = build.Projects;
            build_editor_model.SelectedClient = build_editor_model.Clients
                                        .SingleOrDefault(c => c.Id == build.Client_Id);

            build_editor_window.DataContext = build_editor_model;

            if (build_editor_window.ShowDialog() != true) return false;

            build.Name = build_editor_model.Name;
            build.Adress = build_editor_model.Adress;
            build.Projects = build_editor_model.Project;
            build.Client_Id = build_editor_model.SelectedClient.Id;

            return true;
        }

        private static bool EditCompany(Companies company)
        {
            var company_editor_window = App.Host.Services
                                        .GetRequiredService<CompanyEditorWindow>();
            var company_editor_model = App.Host.Services
                                        .GetRequiredService<CompanyEditorViewModel>();

            company_editor_model.Name = company.Name;
            company_editor_model.Address = company.Adress;
            company_editor_model.Phone = company.Phone;
            company_editor_model.Email = company.Mail;
            company_editor_model.CompanyPositions = company.Positions;

            company_editor_window.DataContext = company_editor_model;

            if (company_editor_window.ShowDialog() != true) return false;

            company.Name = company_editor_model.Name;
            company.Adress = company_editor_model.Address;
            company.Phone = company_editor_model.Phone;
            company.Mail = company_editor_model.Email;
            company.Positions = company_editor_model.CompanyPositions;

            return true;
        }

        private static bool EditEmploee(Employees employee)
        {
            var employee_editor_window = App.Host.Services
                                        .GetRequiredService<EmployeeEditorWindow>();
            var employee_editor_model = App.Host.Services
                                        .GetRequiredService<EmployeeEditorViewModel>();

            employee_editor_model.UserLogin = employee.UsersData.Login;
            employee_editor_model.UserPassword = employee.UsersData.Password;
            employee_editor_model.UserName = employee.First_Name;
            employee_editor_model.UserSurName = employee.Last_Name;
            employee_editor_model.UserEmail = employee.Mail;
            employee_editor_model.UserPhone = employee.Phone;
            employee_editor_model.SelectedCompany = employee.Companies;
            employee_editor_model.SelectedPosition = employee.Positions;

            employee_editor_window.DataContext = employee_editor_model;

            if (employee_editor_window.ShowDialog() != true) return false;

            employee.UsersData.Login = employee_editor_model.UserLogin;
            employee.UsersData.Password = employee_editor_model.UserPassword;
            employee.First_Name = employee_editor_model.UserName;
            employee.Last_Name = employee_editor_model.UserSurName;
            employee.Mail = employee_editor_model.UserEmail;
            employee.Phone = employee_editor_model.UserPhone;
            employee.Companies = employee_editor_model.SelectedCompany;
            employee.Positions = employee_editor_model.SelectedPosition;

            return true;
        }

        private static bool EditPosition(Positions position)
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

        private static bool EditSection(Sections section)
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

        private static bool EditUnit(Units unit)
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

        private static bool EditSupplier(Suppliers supplier)
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

        private static bool EditCategory(Categories category)
        {
            var category_editor_window = App.Host.Services
                                       .GetRequiredService<CategoryEditorWindow>();
            var category_editor_model = App.Host.Services
                                        .GetRequiredService<CategoryEditorViewModel>();

            category_editor_model.Name = category.Name;
            category_editor_model.CategorySections = category.Sections;

            category_editor_window.DataContext = category_editor_model;

            if (category_editor_window.ShowDialog() != true) return false;

            category.Name = category_editor_model.Name;
            category.Sections = category_editor_model.CategorySections;

            return true;
        }

        private static bool EditProduct(Products product)
        {
            var product_editor_window = App.Host.Services
                                       .GetRequiredService<ProductEditorWindow>();
            var product_editor_model = App.Host.Services
                                        .GetRequiredService<ProductEditorViewModel>();

            product_editor_model.Name = product.Name;
            product_editor_model.Model = product.Model;
            product_editor_model.EntryPrice = product.Entry_Price.Value;
            product_editor_model.SelectedUnit = product.Units;
            product_editor_model.SelectedCategory = product.Categories;
            product_editor_model.SelectedManufacturer = product.Manufacturers;
            product_editor_model.ProductSuppliers = product.Suppliers;

            product_editor_window.DataContext = product_editor_model;

            if (product_editor_window.ShowDialog() != true) return false;

            product.Name = product_editor_model.Name;
            product.Model = product_editor_model.Model;
            product.Entry_Price = product_editor_model.EntryPrice;
            product.Units = product_editor_model.SelectedUnit;
            product.Categories = product_editor_model.SelectedCategory;
            product.Manufacturers = product_editor_model.SelectedManufacturer;

            return true;
        }

        private static bool EditInstall(Installs install)
        {
            var install_editor_window = App.Host.Services
                                       .GetRequiredService<InstallEditorWindow>();
            var install_editor_model = App.Host.Services
                                        .GetRequiredService<InstallEditorViewModel>();

            install_editor_model.Name = install.Name;
            install_editor_model.EntryPrice = install.Entry_Price.Value;
            install_editor_model.SelectedUnit = install.Units;
            install_editor_model.SelectedCategory = install.Categories;

            install_editor_window.DataContext = install_editor_model;

            if (install_editor_window.ShowDialog() != true) return false;

            install.Name = install_editor_model.Name;
            install.Entry_Price = install_editor_model.EntryPrice;
            install.Units = install_editor_model.SelectedUnit;
            install.Categories = install_editor_model.SelectedCategory;

            return true;
        }

        private static bool EditManufacturer(Manufacturers manufacturer)
        {
            var manufacturer_editor_window = App.Host.Services
                                       .GetRequiredService<ManufacturerEditorWindow>();
            var manufacturer_editor_model = App.Host.Services
                                       .GetRequiredService<ManufacturerEditorViewModel>();

            manufacturer_editor_model.Name = manufacturer.Name;

            manufacturer_editor_window.DataContext = manufacturer_editor_model;

            if (manufacturer_editor_window.ShowDialog() != true) return false;

            manufacturer.Name = manufacturer_editor_model.Name;

            return true;
        }

        private static bool EditOffer(Offers offer)
        {
            var offer_intit_window = App.Host.Services
                                        .GetRequiredService<OfferInitWindow>();
            var offer_init_model = App.Host.Services
                                        .GetRequiredService<OfferInitViewModel>();

            offer_init_model.NameProject = offer.Projects.Name;
            offer_init_model.Name = offer.Name;
            offer_init_model.SelectedSection = offer.Sections;
            offer_init_model.MarginProduct = offer.Configs.Margin_Product;
            offer_init_model.MarginInstall = offer.Configs.Margin_Work;
            offer_init_model.MarginAdmin = offer.Configs.Margin_Admin;

            offer_intit_window.DataContext = offer_init_model;

            if (offer_intit_window.ShowDialog() != true) return false;

            offer.Name = offer_init_model.Name;
            offer.Sections = offer_init_model.SelectedSection;
            offer.Configs.Margin_Product = offer_init_model.MarginProduct;
            offer.Configs.Margin_Work = offer_init_model.MarginInstall;
            offer.Configs.Margin_Admin = offer_init_model.MarginAdmin;

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
