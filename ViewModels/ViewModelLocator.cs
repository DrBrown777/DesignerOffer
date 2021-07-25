﻿using Designer_Offer.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Designer_Offer.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowView => 
            App.Host.Services.GetRequiredService<MainWindowViewModel>();

        public WorkWindowViewModel WorkWindowView =>
            App.Host.Services.GetRequiredService<WorkWindowViewModel>();

        public ProjectManagerViewModel ProjectManagerView =>
           App.Host.Services.GetRequiredService<ProjectManagerViewModel>();

        public CompanyManagerViewModel CompanyManagerView =>
            App.Host.Services.GetRequiredService<CompanyManagerViewModel>();

        public ServiceManagerViewModel ServiceManagerView =>
            App.Host.Services.GetRequiredService<ServiceManagerViewModel>();

        public LoginViewModel LoginView =>
            (LoginViewModel)App.Host.Services.GetRequiredService<ILoginService>();

        public RegistrationViewModel RegistrationView =>
            (RegistrationViewModel)App.Host.Services.GetRequiredService<IRegistrationService>();

        public ClientEditorViewModel ClientEditorView =>
            App.Host.Services.GetRequiredService<ClientEditorViewModel>();

        public BuildEditorViewModel BuildEditorView =>
            App.Host.Services.GetRequiredService<BuildEditorViewModel>();

        public CompanyEditorViewModel CompanyEditorView =>
            App.Host.Services.GetRequiredService<CompanyEditorViewModel>();

        public EmployeeEditorViewModel EmployeeEditorView =>
            App.Host.Services.GetRequiredService<EmployeeEditorViewModel>();

        public PositionEditorViewModel PositionEditorView =>
            App.Host.Services.GetRequiredService<PositionEditorViewModel>();

        public SectionEditorViewModel SectionEditorView =>
            App.Host.Services.GetRequiredService<SectionEditorViewModel>();

        public UnitEditorViewModel UnitEditorView =>
            App.Host.Services.GetRequiredService<UnitEditorViewModel>();

        public SupplierEditorViewModel SupplierEditorView =>
            App.Host.Services.GetRequiredService<SupplierEditorViewModel>();
    }
}
