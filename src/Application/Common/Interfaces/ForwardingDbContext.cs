using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Application.Common.Interfaces;
public class ForwardingDbContext : AuditableDbContextBase
{
    private readonly string connectionString;
    private readonly IConnectionStringProvider connectionStringProvider;

    // TABLES
    public DbSet<ViewedReleaseNote> ViewedReleaseNotes { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<ContactPersonType> ContactPersonTypes { get; set; }
    public DbSet<MessageType> MessageTypes { get; set; }
    public DbSet<AccountingGroup> AccountingGroups { get; set; }
    public DbSet<PaymentTerm> PaymentTerms { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyType> CompanyTypes { get; set; }
    public DbSet<PartyType> PartyTypes { get; set; }
    public DbSet<ContactPerson> ContactPersons { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<ActivityCode> ActivityCodes { get; set; }
    public DbSet<ContainerIsoCode> ContainerIsoCodes { get; set; }
    public DbSet<OperationalFileContainerSealNumber> OperationalFileContainerSealNumbers { get; set; }
    public DbSet<Depot> Depots { get; set; }
    public DbSet<HsCode> HsCodes { get; set; }
    public DbSet<ImoClassification> ImoClassifications { get; set; }
    public DbSet<Incoterm> Incoterms { get; set; }
    public DbSet<PackageType> PackageTypes { get; set; }
    public DbSet<Port> Ports { get; set; }
    public DbSet<Terminal> Terminals { get; set; }
    public DbSet<ActivityCodeGroup> ActivityCodeGroups { get; set; }
    public DbSet<OperationalFile> OperationalFiles { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<OperationalFileShippingInstructions> OperationalFileShippingInstructions { get; set; }
    public DbSet<InvoiceComment> InvoiceComments { get; set; }
    public DbSet<AccountingNumber> AccountingNumbers { get; set; }
    public DbSet<OperationalFileGeneral> OperationalFileGenerals { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<StepTemplate> StepTemplates { get; set; }
    public DbSet<CompanyAddress> CompanyAddresses { get; set; }
    public DbSet<SupplierIntegration> SupplierIntegrations { get; set; }
    public DbSet<OperationalFileGoods> OperationalFileGoods { get; set; }
    public DbSet<OperationalFileContainer> OperationalFileContainers { get; set; }
    public DbSet<OperationalFileContainerGoods> OperationalFileContainerGoods { get; set; }
    public DbSet<Vessel> Vessels { get; set; }
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<EmailTemplateType> EmailTemplateTypes { get; set; }
    public DbSet<OperationalFileParty> OperationalFileParties { get; set; }
    public DbSet<OperationalFilePartyStructuredCompanyAddress> OperationalFilePartyStructuredCompanyAddresses { get; set; }
    public DbSet<OperationalFileStep> OperationalFileSteps { get; set; }
    public DbSet<OperationalFileCustomsDeclaration> OperationalFileCustomsDeclarations { get; set; }
    public DbSet<IncludedCustomsDocumentType> IncludedCustomsDocumentTypes { get; set; }
    public DbSet<TransportMode> TransportModes { get; set; }
    public DbSet<OperationalFileTransportStop> OperationalFileTransportStops { get; set; }
    public DbSet<OperationalFileTransport> OperationalFileTransports { get; set; }
    public DbSet<TransportStopType> TransportStopTypes { get; set; }
    public DbSet<OperationalFileChosenCustomsDocument> OperationalFileChosenCustomsDocument { get; set; }
    public DbSet<OperationalFileCost> OperationalFileCosts { get; set; }
    public DbSet<OperationalFileRevenue> OperationalFileRevenues { get; set; }
    public DbSet<OutgoingInvoice> OutgoingInvoices { get; set; }
    public DbSet<OutgoingInvoiceLine> OutgoingInvoiceLines { get; set; }
    public DbSet<IncomingInvoice> IncomingInvoices { get; set; }
    public DbSet<InvoiceType> InvoiceTypes { get; set; }
    public DbSet<OutgoingTaxCode> OutgoingTaxCodes { get; set; }
    public DbSet<IncomingTaxCode> IncomingTaxCodes { get; set; }
    public DbSet<IncomingTaxSystem> IncomingTaxSystems { get; set; }
    public DbSet<ValidatedOperationalFileCost> ValidatedOperationalFileCosts { get; set; }
    public DbSet<DocumentTemplate> DocumentTemplates { get; set; }
    public DbSet<DocumentTemplateType> DocumentTemplateTypes { get; set; }
    public DbSet<OperationalFileUser> OperationalFileUsers { get; set; }
    public DbSet<UnitOfMeasurement> UnitsOfMeasurement { get; set; }
    public DbSet<CustomsOffice> CustomsOffices { get; set; }
    public DbSet<CustomsPreviousArrangementType> CustomsPreviousArrangementTypes { get; set; }
    public DbSet<CustomsInvoiceType> CustomsInvoiceTypes { get; set; }
    public DbSet<OperationalFileGeneratedDocument> OperationalFileGeneratedDocuments { get; set; }
    public DbSet<VgmWeighingMethod> VgmWeighingMethods { get; set; }
    public DbSet<ActivityCodeTranslation> ActivityCodeTranslations { get; set; }
    public DbSet<QuoteFile> QuoteFiles { get; set; }
    public DbSet<QuoteFileStatus> QuoteStatuses { get; set; }
    public DbSet<QuoteFileContainer> QuoteFileContainers { get; set; }
    public DbSet<QuoteFileGoods> QuoteFileGoods { get; set; }
    public DbSet<QuoteFileRevenue> QuoteFileRevenues { get; set; }
    public DbSet<QuoteFileCost> QuoteFileCosts { get; set; }
    public DbSet<QuoteFileParty> QuoteFileParties { get; set; }
    public DbSet<AccountingPeriod> AccountingPeriods { get; set; }
    public DbSet<CustomsTaxType> CustomsTaxTypes { get; set; }
    public DbSet<StandardText> StandardTexts { get; set; }
    public DbSet<OperationalFileCourierInstruction> OperationalFileCourierInstruction { get; set; }
    public DbSet<CustomsDeclarationType> CustomsDeclarationTypes { get; set; }
    public DbSet<CustomsDeclarationTypePartOne> CustomsDeclarationTypePartsOne { get; set; }
    public DbSet<CustomsDeclarationTypePartTwo> CustomsDeclarationTypePartsTwo { get; set; }
    public DbSet<CustomsDeclarationProcedureType> CustomsDeclarationProcedureTypes { get; set; }
    public DbSet<CombinedCustomsDeclarationType> CombinedCustomsDeclarationTypes { get; set; }
    public DbSet<ContainerReleaseRight> ContainerReleaseRights { get; set; }
    public DbSet<RequestQuoteClientPortalConfig> RequestQuoteClientPortalConfigs { get; set; }
    public DbSet<OperationalClientPortalConfig> OperationalClientPortalConfigs { get; set; }
    public DbSet<QuoteClientPortalConfig> QuoteClientPortalConfigs { get; set; }
    public DbSet<OperationalFileFreeRemark> OperationalFileFreeRemarks { get; set; }
    public DbSet<DepartmentUser> DepartmentUsers { get; set; }
    public DbSet<Department> Departments { get; set; }

    public DbSet<GridLayoutData> GridLayoutData { get; set; }
    public DbSet<GridLayout> GridLayouts { get; set; }
    public DbSet<GridLayoutFilter> GridLayoutFilters { get; set; }
    public DbSet<GridLayoutDisplayedColumn> GridLayoutDisplayedColumns { get; set; }

    public DbSet<OperationalFileBudgetSummary> OperationalFileBudgetSummaries { get; set; }
    public DbSet<OperationalFileSummary> OperationalFileSummaries { get; set; }

    public DbSet<YearlyBudgetRequest> YearlyBudgetRequests { get; set; }
    public DbSet<YearlyBudgetOverviewEntity> YearlyBudgetOverviews { get; set; }
    public DbSet<OcrSetting> OcrSetting { get; set; }

    // VIEWS
    public DbQuery<FoundCompanyViewEntity> FoundCompanies { get; set; }
    public DbQuery<FoundOperationalFileTransportStopViewEntity> FoundOperationalFileTransportStops { get; set; }
    public DbQuery<FoundBudgetDetailsPerFileEntity> FoundBudgetDetailsPerOperationalFile { get; set; }
    public DbQuery<FoundOperationalFileViewEntity> FoundOperationalFiles { get; set; }
    public DbQuery<FoundOutgoingInvoicesViewEntity> FoundOutgoingInvoices { get; set; }
    public DbQuery<FoundBudgetPerActivityCodeEntity> FoundBudgetPerActivityCodes { get; set; }
    public DbQuery<FoundOperationalSummaryPerUserEntity> FoundOperationalSummaryPerUser { get; set; }
    public DbQuery<BudgetTotalsReportEntity> BudgetTotalsReport { get; set; }

    //FUNCTIONS
    public int DamerauLevenshteinComparison(string input, string compareValue, int? max = null)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.SetDecimalPrecision(8);
        modelBuilder.RemoveCascadeDeleteBehavior();

        Type[] excludedTypes =
        {
                typeof(FoundCompanyViewEntity),
                typeof(FoundOperationalFileTransportStopViewEntity),
                typeof(FoundBudgetDetailsPerFileEntity),
                typeof(FoundOperationalFileViewEntity),
                typeof(FoundOutgoingInvoicesViewEntity),
                typeof(FoundBudgetPerActivityCodeEntity),
                typeof(FoundOperationalSummaryPerUserEntity),
                typeof(BudgetTotalsReportEntity)
            };
        modelBuilder.RegisterSoftDeleteQueryFilter(excludedTypes);
        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.HasDbFunction(this.GetType().GetMethod(nameof(DamerauLevenshteinComparison), new[] { typeof(string), typeof(string), typeof(int?) }))
            .HasName("DamerauLevenshteinComparison");

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();

        if (connectionStringProvider != null)
        {
            string tenantConnectionString = connectionStringProvider.GetAsync<ForwardingDbContext>(CancellationToken.None).GetAwaiter().GetResult();
            optionsBuilder.UseSqlServer(tenantConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    public void RejectChanges()
    {
        foreach (var entry in ChangeTracker.Entries().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                case EntityState.Deleted:
                    entry.State = EntityState.Modified; //Revert changes made to deleted entity.
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
            }
        }
    }
}
