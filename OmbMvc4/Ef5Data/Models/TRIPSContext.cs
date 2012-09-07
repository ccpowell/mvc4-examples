using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Ef5Data.Models.Mapping;

namespace Ef5Data.Models
{
    public class TRIPSContext : DbContext
    {
        static TRIPSContext()
        {
            Database.SetInitializer<TRIPSContext>(null);
        }

		public TRIPSContext()
			: base("Name=TRIPSContext")
		{
		}

        public DbSet<Processed> Processeds { get; set; }
        public DbSet<Raw> Raws { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportOverride> ReportOverrides { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<AdhocProjectVersion> AdhocProjectVersions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryType> CategoryTypes { get; set; }
        public DbSet<CDOTCorridorGISCategory> CDOTCorridorGISCategories { get; set; }
        public DbSet<CDOTInvestmentTypeCategory> CDOTInvestmentTypeCategories { get; set; }
        public DbSet<CountyGeography> CountyGeographies { get; set; }
        public DbSet<Cycle> Cycles { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<EvaluationCriterion> EvaluationCriterions { get; set; }
        public DbSet<EvaluationCriterionProjectType> EvaluationCriterionProjectTypes { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<FeatureType> FeatureTypes { get; set; }
        public DbSet<FundingGroup> FundingGroups { get; set; }
        public DbSet<FundingIncrement> FundingIncrements { get; set; }
        public DbSet<FundingLevel> FundingLevels { get; set; }
        public DbSet<FundingResource> FundingResources { get; set; }
        public DbSet<FundingResourceAmount> FundingResourceAmounts { get; set; }
        public DbSet<FundingResourceTimePeriod> FundingResourceTimePeriods { get; set; }
        public DbSet<FundingType> FundingTypes { get; set; }
        public DbSet<FundingTypeLevel> FundingTypeLevels { get; set; }
        public DbSet<Geography> Geographies { get; set; }
        public DbSet<GeographyType> GeographyTypes { get; set; }
        public DbSet<GISCategory> GISCategories { get; set; }
        public DbSet<GISCategoryType> GISCategoryTypes { get; set; }
        public DbSet<ImprovementType> ImprovementTypes { get; set; }
        public DbSet<LoginProfile> LoginProfiles { get; set; }
        public DbSet<LR> LRS { get; set; }
        public DbSet<LRSCategory> LRSCategories { get; set; }
        public DbSet<LRSCategoryType> LRSCategoryTypes { get; set; }
        public DbSet<LRSProjectsLookup> LRSProjectsLookups { get; set; }
        public DbSet<LRSScheme> LRSSchemes { get; set; }
        public DbSet<LRSSchemeAttr> LRSSchemeAttrs { get; set; }
        public DbSet<Medium> Media { get; set; }
        public DbSet<MetroVisionMeasure> MetroVisionMeasures { get; set; }
        public DbSet<MetroVisionMeasureSponsor> MetroVisionMeasureSponsors { get; set; }
        public DbSet<MuniGeography> MuniGeographies { get; set; }
        public DbSet<Network> Networks { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationPersonRole> OrganizationPersonRoles { get; set; }
        public DbSet<OrganizationType> OrganizationTypes { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PoolProject> PoolProjects { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProfileAttribute> ProfileAttributes { get; set; }
        public DbSet<ProfileAttributeType> ProfileAttributeTypes { get; set; }
        public DbSet<ProfileAttributeTypeClass> ProfileAttributeTypeClasses { get; set; }
        public DbSet<ProfileType> ProfileTypes { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProgramInstance> ProgramInstances { get; set; }
        public DbSet<ProgramInstanceProjectType> ProgramInstanceProjectTypes { get; set; }
        public DbSet<ProgramInstanceSponsor> ProgramInstanceSponsors { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectCDOTData> ProjectCDOTDatas { get; set; }
        public DbSet<ProjectCountyGeography> ProjectCountyGeographies { get; set; }
        public DbSet<ProjectFinancialRecord> ProjectFinancialRecords { get; set; }
        public DbSet<ProjectFinancialRecordDetail> ProjectFinancialRecordDetails { get; set; }
        public DbSet<ProjectFinancialRecordDetailPhase> ProjectFinancialRecordDetailPhases { get; set; }
        public DbSet<ProjectModelCoding> ProjectModelCodings { get; set; }
        public DbSet<ProjectMuniGeography> ProjectMuniGeographies { get; set; }
        public DbSet<ProjectPool> ProjectPools { get; set; }
        public DbSet<ProjectSegment> ProjectSegments { get; set; }
        public DbSet<ProjectSponsor> ProjectSponsors { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<ProjectVersion> ProjectVersions { get; set; }
        public DbSet<ProjectVersionReporting> ProjectVersionReportings { get; set; }
        public DbSet<Report1> Report1 { get; set; }
        public DbSet<ReportProjectVersionSorting> ReportProjectVersionSortings { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleType> RoleTypes { get; set; }
        public DbSet<RTPImprovementType> RTPImprovementTypes { get; set; }
        public DbSet<RTPProgramInstance> RTPProgramInstances { get; set; }
        public DbSet<RTPProgramInstanceSponsor> RTPProgramInstanceSponsors { get; set; }
        public DbSet<RTPProjectVersion> RTPProjectVersions { get; set; }
        public DbSet<RTPReportGroupingCategory> RTPReportGroupingCategories { get; set; }
        public DbSet<Security> Securities { get; set; }
        public DbSet<SponsorOrganization> SponsorOrganizations { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<StreetAddress> StreetAddresses { get; set; }
        public DbSet<Strike> Strikes { get; set; }
        public DbSet<SurveyProgramInstance> SurveyProgramInstances { get; set; }
        public DbSet<SurveyProgramInstanceSponsor> SurveyProgramInstanceSponsors { get; set; }
        public DbSet<SurveyProjectVersion> SurveyProjectVersions { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<TimePeriod> TimePeriods { get; set; }
        public DbSet<TimePeriodCycle> TimePeriodCycles { get; set; }
        public DbSet<TimePeriodType> TimePeriodTypes { get; set; }
        public DbSet<TipImprovementType> TipImprovementTypes { get; set; }
        public DbSet<TIPProgramInstance> TIPProgramInstances { get; set; }
        public DbSet<TIPProgramInstanceSponsor> TIPProgramInstanceSponsors { get; set; }
        public DbSet<TIPProjectEvaluation2010> TIPProjectEvaluation2010 { get; set; }
        public DbSet<TIPProjectEvaluation2010Mode> TIPProjectEvaluation2010Mode { get; set; }
        public DbSet<TIPProjectEvaluation2010ServiceType> TIPProjectEvaluation2010ServiceType { get; set; }
        public DbSet<TIPProjectVersion> TIPProjectVersions { get; set; }
        public DbSet<TIPProjectVersionArchive> TIPProjectVersionArchives { get; set; }
        public DbSet<CycleProjectVersion> CycleProjectVersions { get; set; }
        public DbSet<PlanSurvey> PlanSurveys { get; set; }
        public DbSet<GetFundingResource> GetFundingResources { get; set; }
        public DbSet<v_GetFullyDepictedPoolProjects> v_GetFullyDepictedPoolProjects { get; set; }
        public DbSet<validation_GetProject> validation_GetProject { get; set; }
        public DbSet<validation_GetProjectSponsors> validation_GetProjectSponsors { get; set; }
        public DbSet<vw_Sponsors> vw_Sponsors { get; set; }
        public DbSet<vw_TipProgram> vw_TipProgram { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ProcessedMap());
            modelBuilder.Configurations.Add(new RawMap());
            modelBuilder.Configurations.Add(new ReportMap());
            modelBuilder.Configurations.Add(new ReportOverrideMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressTypeMap());
            modelBuilder.Configurations.Add(new AdhocProjectVersionMap());
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new CategoryTypeMap());
            modelBuilder.Configurations.Add(new CDOTCorridorGISCategoryMap());
            modelBuilder.Configurations.Add(new CDOTInvestmentTypeCategoryMap());
            modelBuilder.Configurations.Add(new CountyGeographyMap());
            modelBuilder.Configurations.Add(new CycleMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new EvaluationCriterionMap());
            modelBuilder.Configurations.Add(new EvaluationCriterionProjectTypeMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new FeatureTypeMap());
            modelBuilder.Configurations.Add(new FundingGroupMap());
            modelBuilder.Configurations.Add(new FundingIncrementMap());
            modelBuilder.Configurations.Add(new FundingLevelMap());
            modelBuilder.Configurations.Add(new FundingResourceMap());
            modelBuilder.Configurations.Add(new FundingResourceAmountMap());
            modelBuilder.Configurations.Add(new FundingResourceTimePeriodMap());
            modelBuilder.Configurations.Add(new FundingTypeMap());
            modelBuilder.Configurations.Add(new FundingTypeLevelMap());
            modelBuilder.Configurations.Add(new GeographyMap());
            modelBuilder.Configurations.Add(new GeographyTypeMap());
            modelBuilder.Configurations.Add(new GISCategoryMap());
            modelBuilder.Configurations.Add(new GISCategoryTypeMap());
            modelBuilder.Configurations.Add(new ImprovementTypeMap());
            modelBuilder.Configurations.Add(new LoginProfileMap());
            modelBuilder.Configurations.Add(new LRMap());
            modelBuilder.Configurations.Add(new LRSCategoryMap());
            modelBuilder.Configurations.Add(new LRSCategoryTypeMap());
            modelBuilder.Configurations.Add(new LRSProjectsLookupMap());
            modelBuilder.Configurations.Add(new LRSSchemeMap());
            modelBuilder.Configurations.Add(new LRSSchemeAttrMap());
            modelBuilder.Configurations.Add(new MediumMap());
            modelBuilder.Configurations.Add(new MetroVisionMeasureMap());
            modelBuilder.Configurations.Add(new MetroVisionMeasureSponsorMap());
            modelBuilder.Configurations.Add(new MuniGeographyMap());
            modelBuilder.Configurations.Add(new NetworkMap());
            modelBuilder.Configurations.Add(new OrganizationMap());
            modelBuilder.Configurations.Add(new OrganizationPersonRoleMap());
            modelBuilder.Configurations.Add(new OrganizationTypeMap());
            modelBuilder.Configurations.Add(new PersonMap());
            modelBuilder.Configurations.Add(new PoolProjectMap());
            modelBuilder.Configurations.Add(new ProfileMap());
            modelBuilder.Configurations.Add(new ProfileAttributeMap());
            modelBuilder.Configurations.Add(new ProfileAttributeTypeMap());
            modelBuilder.Configurations.Add(new ProfileAttributeTypeClassMap());
            modelBuilder.Configurations.Add(new ProfileTypeMap());
            modelBuilder.Configurations.Add(new ProgramMap());
            modelBuilder.Configurations.Add(new ProgramInstanceMap());
            modelBuilder.Configurations.Add(new ProgramInstanceProjectTypeMap());
            modelBuilder.Configurations.Add(new ProgramInstanceSponsorMap());
            modelBuilder.Configurations.Add(new ProjectMap());
            modelBuilder.Configurations.Add(new ProjectCDOTDataMap());
            modelBuilder.Configurations.Add(new ProjectCountyGeographyMap());
            modelBuilder.Configurations.Add(new ProjectFinancialRecordMap());
            modelBuilder.Configurations.Add(new ProjectFinancialRecordDetailMap());
            modelBuilder.Configurations.Add(new ProjectFinancialRecordDetailPhaseMap());
            modelBuilder.Configurations.Add(new ProjectModelCodingMap());
            modelBuilder.Configurations.Add(new ProjectMuniGeographyMap());
            modelBuilder.Configurations.Add(new ProjectPoolMap());
            modelBuilder.Configurations.Add(new ProjectSegmentMap());
            modelBuilder.Configurations.Add(new ProjectSponsorMap());
            modelBuilder.Configurations.Add(new ProjectTypeMap());
            modelBuilder.Configurations.Add(new ProjectVersionMap());
            modelBuilder.Configurations.Add(new ProjectVersionReportingMap());
            modelBuilder.Configurations.Add(new Report1Map());
            modelBuilder.Configurations.Add(new ReportProjectVersionSortingMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RoleTypeMap());
            modelBuilder.Configurations.Add(new RTPImprovementTypeMap());
            modelBuilder.Configurations.Add(new RTPProgramInstanceMap());
            modelBuilder.Configurations.Add(new RTPProgramInstanceSponsorMap());
            modelBuilder.Configurations.Add(new RTPProjectVersionMap());
            modelBuilder.Configurations.Add(new RTPReportGroupingCategoryMap());
            modelBuilder.Configurations.Add(new SecurityMap());
            modelBuilder.Configurations.Add(new SponsorOrganizationMap());
            modelBuilder.Configurations.Add(new StatusMap());
            modelBuilder.Configurations.Add(new StatusTypeMap());
            modelBuilder.Configurations.Add(new StreetAddressMap());
            modelBuilder.Configurations.Add(new StrikeMap());
            modelBuilder.Configurations.Add(new SurveyProgramInstanceMap());
            modelBuilder.Configurations.Add(new SurveyProgramInstanceSponsorMap());
            modelBuilder.Configurations.Add(new SurveyProjectVersionMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new TimePeriodMap());
            modelBuilder.Configurations.Add(new TimePeriodCycleMap());
            modelBuilder.Configurations.Add(new TimePeriodTypeMap());
            modelBuilder.Configurations.Add(new TipImprovementTypeMap());
            modelBuilder.Configurations.Add(new TIPProgramInstanceMap());
            modelBuilder.Configurations.Add(new TIPProgramInstanceSponsorMap());
            modelBuilder.Configurations.Add(new TIPProjectEvaluation2010Map());
            modelBuilder.Configurations.Add(new TIPProjectEvaluation2010ModeMap());
            modelBuilder.Configurations.Add(new TIPProjectEvaluation2010ServiceTypeMap());
            modelBuilder.Configurations.Add(new TIPProjectVersionMap());
            modelBuilder.Configurations.Add(new TIPProjectVersionArchiveMap());
            modelBuilder.Configurations.Add(new CycleProjectVersionMap());
            modelBuilder.Configurations.Add(new PlanSurveyMap());
            modelBuilder.Configurations.Add(new GetFundingResourceMap());
            modelBuilder.Configurations.Add(new v_GetFullyDepictedPoolProjectsMap());
            modelBuilder.Configurations.Add(new validation_GetProjectMap());
            modelBuilder.Configurations.Add(new validation_GetProjectSponsorsMap());
            modelBuilder.Configurations.Add(new vw_SponsorsMap());
            modelBuilder.Configurations.Add(new vw_TipProgramMap());
        }
    }
}
