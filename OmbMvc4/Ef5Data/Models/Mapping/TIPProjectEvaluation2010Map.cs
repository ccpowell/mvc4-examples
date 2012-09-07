using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Ef5Data.Models.Mapping
{
    public class TIPProjectEvaluation2010Map : EntityTypeConfiguration<TIPProjectEvaluation2010>
    {
        public TIPProjectEvaluation2010Map()
        {
            // Primary Key
            this.HasKey(t => t.ProjectVersionID);

            // Properties
            this.Property(t => t.ProjectVersionID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ServiceType)
                .HasMaxLength(100);

            this.Property(t => t.SecondaryServiceType)
                .HasMaxLength(100);

            this.Property(t => t.SpeedLimit)
                .HasMaxLength(50);

            this.Property(t => t.EligibleCapacityProject)
                .HasMaxLength(100);

            this.Property(t => t.BikeAndPedSubtype)
                .HasMaxLength(10);

            this.Property(t => t.AQImprovementMethod)
                .HasMaxLength(50);

            this.Property(t => t.PollutantType)
                .HasMaxLength(50);

            this.Property(t => t.ProjectLocationForPM10)
                .HasMaxLength(50);

            this.Property(t => t.EmphasisType)
                .HasMaxLength(50);

            this.Property(t => t.GapClosureMeasure)
                .HasMaxLength(50);

            this.Property(t => t.PedCrossingMeasure)
                .HasMaxLength(50);

            this.Property(t => t.PriorityCorridorsBikePedProjectType)
                .HasMaxLength(100);

            this.Property(t => t.BikePedConnectivityGapClosure)
                .HasMaxLength(255);

            this.Property(t => t.BikePedConnectivityAccess)
                .HasMaxLength(255);

            this.Property(t => t.BikePedConnectivityBarrierElimination)
                .HasMaxLength(255);

            this.Property(t => t.BikePedConnectivityTransit)
                .HasMaxLength(255);

            this.Property(t => t.WithinUrbanCenter)
                .HasMaxLength(75);

            this.Property(t => t.StrategicCorridors)
                .HasMaxLength(75);

            this.Property(t => t.BusServiceClass)
                .HasMaxLength(50);

            this.Property(t => t.EmphasisCorridors)
                .HasMaxLength(50);

            this.Property(t => t.NameOfQualifiedTrafficEngineer)
                .HasMaxLength(100);

            this.Property(t => t.withinugb)
                .HasMaxLength(20);

            this.Property(t => t.bikepedconnectivitylocation)
                .HasMaxLength(255);

            this.Property(t => t.StudiesProjectType)
                .HasMaxLength(50);

            this.Property(t => t.Temp_ResubmitReadyStatus)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("TIPProjectEvaluation2010");
            this.Property(t => t.ProjectVersionID).HasColumnName("ProjectVersionID");
            this.Property(t => t.PCI).HasColumnName("PCI");
            this.Property(t => t.PavementConditionScore).HasColumnName("PavementConditionScore");
            this.Property(t => t.TotalScore).HasColumnName("TotalScore");
            this.Property(t => t.SafetyScore).HasColumnName("SafetyScore");
            this.Property(t => t.FatalCrashes).HasColumnName("FatalCrashes");
            this.Property(t => t.InjuryCrashes).HasColumnName("InjuryCrashes");
            this.Property(t => t.PDOCrashes).HasColumnName("PDOCrashes");
            this.Property(t => t.AccidentsReduced).HasColumnName("AccidentsReduced");
            this.Property(t => t.ServiceType).HasColumnName("ServiceType");
            this.Property(t => t.SecondaryServiceType).HasColumnName("SecondaryServiceType");
            this.Property(t => t.AWDT).HasColumnName("AWDT");
            this.Property(t => t.AWDT2).HasColumnName("AWDT2");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.NumberOfLanes).HasColumnName("NumberOfLanes");
            this.Property(t => t.UsageScore).HasColumnName("UsageScore");
            this.Property(t => t.CEScore).HasColumnName("CEScore");
            this.Property(t => t.VOverC).HasColumnName("VOverC");
            this.Property(t => t.CongestionScore).HasColumnName("CongestionScore");
            this.Property(t => t.PriorityScore).HasColumnName("PriorityScore");
            this.Property(t => t.VHT).HasColumnName("VHT");
            this.Property(t => t.ContinuityScore).HasColumnName("ContinuityScore");
            this.Property(t => t.MetroVisionProjectScore).HasColumnName("MetroVisionProjectScore");
            this.Property(t => t.PlanImplementation).HasColumnName("PlanImplementation");
            this.Property(t => t.EmphasizedRoadway).HasColumnName("EmphasizedRoadway");
            this.Property(t => t.Usage).HasColumnName("Usage");
            this.Property(t => t.SpeedLimit).HasColumnName("SpeedLimit");
            this.Property(t => t.InsufficientLight).HasColumnName("InsufficientLight");
            this.Property(t => t.ImprovedLight).HasColumnName("ImprovedLight");
            this.Property(t => t.VMT).HasColumnName("VMT");
            this.Property(t => t.Volume).HasColumnName("Volume");
            this.Property(t => t.Capacity).HasColumnName("Capacity");
            this.Property(t => t.AddedAWDT).HasColumnName("AddedAWDT");
            this.Property(t => t.Intersection).HasColumnName("Intersection");
            this.Property(t => t.Length2).HasColumnName("Length2");
            this.Property(t => t.Subsidy).HasColumnName("Subsidy");
            this.Property(t => t.UsageSupport).HasColumnName("UsageSupport");
            this.Property(t => t.UsageSupportScore).HasColumnName("UsageSupportScore");
            this.Property(t => t.BusMarketing).HasColumnName("BusMarketing");
            this.Property(t => t.BusPedAccess).HasColumnName("BusPedAccess");
            this.Property(t => t.BusCostReduction).HasColumnName("BusCostReduction");
            this.Property(t => t.PedAccessAction1).HasColumnName("PedAccessAction1");
            this.Property(t => t.PedAccessAction2).HasColumnName("PedAccessAction2");
            this.Property(t => t.CostReductionAction1).HasColumnName("CostReductionAction1");
            this.Property(t => t.CostReductionAction2).HasColumnName("CostReductionAction2");
            this.Property(t => t.RegionalRelevanceScore).HasColumnName("RegionalRelevanceScore");
            this.Property(t => t.EligibleCapacityProject).HasColumnName("EligibleCapacityProject");
            this.Property(t => t.BikeAndPedSubtype).HasColumnName("BikeAndPedSubtype");
            this.Property(t => t.PotentialNeedScore).HasColumnName("PotentialNeedScore");
            this.Property(t => t.AQImprovementMethod).HasColumnName("AQImprovementMethod");
            this.Property(t => t.BenefitScore).HasColumnName("BenefitScore");
            this.Property(t => t.BenefitFeatureTypeID).HasColumnName("BenefitFeatureTypeID");
            this.Property(t => t.VHTReduction).HasColumnName("VHTReduction");
            this.Property(t => t.VMTReduction).HasColumnName("VMTReduction");
            this.Property(t => t.ReductionInPounds).HasColumnName("ReductionInPounds");
            this.Property(t => t.PollutantType).HasColumnName("PollutantType");
            this.Property(t => t.NetReductionInPounds).HasColumnName("NetReductionInPounds");
            this.Property(t => t.LongTermFunding).HasColumnName("LongTermFunding");
            this.Property(t => t.LongTermFundingScore).HasColumnName("LongTermFundingScore");
            this.Property(t => t.ProvidesMedians).HasColumnName("ProvidesMedians");
            this.Property(t => t.ImprovesAccessControl).HasColumnName("ImprovesAccessControl");
            this.Property(t => t.ProvidesLeftTurns).HasColumnName("ProvidesLeftTurns");
            this.Property(t => t.ImprovesSignalInterconnection).HasColumnName("ImprovesSignalInterconnection");
            this.Property(t => t.ProvidesITS).HasColumnName("ProvidesITS");
            this.Property(t => t.TSMScore).HasColumnName("TSMScore");
            this.Property(t => t.MMCTransitOps).HasColumnName("MMCTransitOps");
            this.Property(t => t.MMCTransitAmenities).HasColumnName("MMCTransitAmenities");
            this.Property(t => t.MMCBikeLanes).HasColumnName("MMCBikeLanes");
            this.Property(t => t.MMCGradeSeparation).HasColumnName("MMCGradeSeparation");
            this.Property(t => t.MMCBikeAmenities).HasColumnName("MMCBikeAmenities");
            this.Property(t => t.MMCPedLinks).HasColumnName("MMCPedLinks");
            this.Property(t => t.MMCSignalPriority).HasColumnName("MMCSignalPriority");
            this.Property(t => t.MMCParking).HasColumnName("MMCParking");
            this.Property(t => t.MMCTMO).HasColumnName("MMCTMO");
            this.Property(t => t.MMCTelework).HasColumnName("MMCTelework");
            this.Property(t => t.MMCRoadwayOps).HasColumnName("MMCRoadwayOps");
            this.Property(t => t.MMCITS).HasColumnName("MMCITS");
            this.Property(t => t.MMCBikeLinks).HasColumnName("MMCBikeLinks");
            this.Property(t => t.MMCShuttles).HasColumnName("MMCShuttles");
            this.Property(t => t.MMCConnections).HasColumnName("MMCConnections");
            this.Property(t => t.MMCServiceGap).HasColumnName("MMCServiceGap");
            this.Property(t => t.MMCRoutes).HasColumnName("MMCRoutes");
            this.Property(t => t.MMCFutureFacilities).HasColumnName("MMCFutureFacilities");
            this.Property(t => t.MultiModalScore).HasColumnName("MultiModalScore");
            this.Property(t => t.MMCRoutes2).HasColumnName("MMCRoutes2");
            this.Property(t => t.MetroVisionScore).HasColumnName("MetroVisionScore");
            this.Property(t => t.OvermatchScore).HasColumnName("OvermatchScore");
            this.Property(t => t.ProjectLocationForPM10).HasColumnName("ProjectLocationForPM10");
            this.Property(t => t.PM10Score).HasColumnName("PM10Score");
            this.Property(t => t.BenefitYears).HasColumnName("BenefitYears");
            this.Property(t => t.EmphasisType).HasColumnName("EmphasisType");
            this.Property(t => t.PMT).HasColumnName("PMT");
            this.Property(t => t.GapClosureMeasure).HasColumnName("GapClosureMeasure");
            this.Property(t => t.PedCrossingMeasure).HasColumnName("PedCrossingMeasure");
            this.Property(t => t.BidirectionalUse).HasColumnName("BidirectionalUse");
            this.Property(t => t.BikePedNetworkConnections).HasColumnName("BikePedNetworkConnections");
            this.Property(t => t.BarrierElimination).HasColumnName("BarrierElimination");
            this.Property(t => t.AccessToTransit).HasColumnName("AccessToTransit");
            this.Property(t => t.Lockers).HasColumnName("Lockers");
            this.Property(t => t.ScenicEasements).HasColumnName("ScenicEasements");
            this.Property(t => t.RailsToTrails).HasColumnName("RailsToTrails");
            this.Property(t => t.ScenicBeautification).HasColumnName("ScenicBeautification");
            this.Property(t => t.ReductionInPoundsAll).HasColumnName("ReductionInPoundsAll");
            this.Property(t => t.ConformityCommitmentPoints).HasColumnName("ConformityCommitmentPoints");
            this.Property(t => t.currentpracticepoints).HasColumnName("currentpracticepoints");
            this.Property(t => t.PriorityCorridorsBikePed).HasColumnName("PriorityCorridorsBikePed");
            this.Property(t => t.PriorityCorridorsBikePedProjectType).HasColumnName("PriorityCorridorsBikePedProjectType");
            this.Property(t => t.BikePedConnectivityGapClosure).HasColumnName("BikePedConnectivityGapClosure");
            this.Property(t => t.BikePedConnectivityAccess).HasColumnName("BikePedConnectivityAccess");
            this.Property(t => t.BikePedConnectivityBarrierElimination).HasColumnName("BikePedConnectivityBarrierElimination");
            this.Property(t => t.BikePedConnectivityTransit).HasColumnName("BikePedConnectivityTransit");
            this.Property(t => t.ConnectivityScore).HasColumnName("ConnectivityScore");
            this.Property(t => t.BikePedBidirectionalUse).HasColumnName("BikePedBidirectionalUse");
            this.Property(t => t.LockersOrRacksPoints).HasColumnName("LockersOrRacksPoints");
            this.Property(t => t.BikePedAcquiringScenicEasements).HasColumnName("BikePedAcquiringScenicEasements");
            this.Property(t => t.BikePedRailsToTrails).HasColumnName("BikePedRailsToTrails");
            this.Property(t => t.MultipleEnhancementsScore).HasColumnName("MultipleEnhancementsScore");
            this.Property(t => t.WithinUrbanCenter).HasColumnName("WithinUrbanCenter");
            this.Property(t => t.DIA).HasColumnName("DIA");
            this.Property(t => t.StrategicCorridors).HasColumnName("StrategicCorridors");
            this.Property(t => t.BusServiceClass).HasColumnName("BusServiceClass");
            this.Property(t => t.ProvidesIncidentManagement).HasColumnName("ProvidesIncidentManagement");
            this.Property(t => t.FutureTransitFacilities).HasColumnName("FutureTransitFacilities");
            this.Property(t => t.TransitAmenities).HasColumnName("TransitAmenities");
            this.Property(t => t.BuildingNewPath).HasColumnName("BuildingNewPath");
            this.Property(t => t.BikeAmenities).HasColumnName("BikeAmenities");
            this.Property(t => t.PedestrianLinks).HasColumnName("PedestrianLinks");
            this.Property(t => t.IncorporatingTransit).HasColumnName("IncorporatingTransit");
            this.Property(t => t.TransitOperationalFeatures).HasColumnName("TransitOperationalFeatures");
            this.Property(t => t.RidershipPotentialScore).HasColumnName("RidershipPotentialScore");
            this.Property(t => t.RidershipPotential).HasColumnName("RidershipPotential");
            this.Property(t => t.PercentBrownfields).HasColumnName("PercentBrownfields");
            this.Property(t => t.PercentInfillRedevelopment).HasColumnName("PercentInfillRedevelopment");
            this.Property(t => t.PercentLowIncome).HasColumnName("PercentLowIncome");
            this.Property(t => t.ExistingStationScore).HasColumnName("ExistingStationScore");
            this.Property(t => t.OtherCriticalityCriteriaScore).HasColumnName("OtherCriticalityCriteriaScore");
            this.Property(t => t.ConditionMajorStructureScore).HasColumnName("ConditionMajorStructureScore");
            this.Property(t => t.RTPScore).HasColumnName("RTPScore");
            this.Property(t => t.BridgeScore).HasColumnName("BridgeScore");
            this.Property(t => t.PMTCostBus).HasColumnName("PMTCostBus");
            this.Property(t => t.PMTCostInterchange).HasColumnName("PMTCostInterchange");
            this.Property(t => t.WeightedCrashRate).HasColumnName("WeightedCrashRate");
            this.Property(t => t.EmphasisCorridors).HasColumnName("EmphasisCorridors");
            this.Property(t => t.ImplementFastracks).HasColumnName("ImplementFastracks");
            this.Property(t => t.LongRangePlanScore).HasColumnName("LongRangePlanScore");
            this.Property(t => t.MetroVisionMeasuresProject).HasColumnName("MetroVisionMeasuresProject");
            this.Property(t => t.ExistingFacilitiesDescription).HasColumnName("ExistingFacilitiesDescription");
            this.Property(t => t.RTPLongRangePlanScore).HasColumnName("RTPLongRangePlanScore");
            this.Property(t => t.NumPropertyOwners).HasColumnName("NumPropertyOwners");
            this.Property(t => t.CurrentCongestionScore).HasColumnName("CurrentCongestionScore");
            this.Property(t => t.NameOfQualifiedTrafficEngineer).HasColumnName("NameOfQualifiedTrafficEngineer");
            this.Property(t => t.mmcpedstreetlighting).HasColumnName("mmcpedstreetlighting");
            this.Property(t => t.mmcstreettrees).HasColumnName("mmcstreettrees");
            this.Property(t => t.mmcdetaching).HasColumnName("mmcdetaching");
            this.Property(t => t.withinugb).HasColumnName("withinugb");
            this.Property(t => t.DieselRetrofitProject).HasColumnName("DieselRetrofitProject");
            this.Property(t => t.ProjectTypeScore).HasColumnName("ProjectTypeScore");
            this.Property(t => t.newconstructionproject).HasColumnName("newconstructionproject");
            this.Property(t => t.compliantlighting).HasColumnName("compliantlighting");
            this.Property(t => t.AdditionalAmenities).HasColumnName("AdditionalAmenities");
            this.Property(t => t.NumRacks).HasColumnName("NumRacks");
            this.Property(t => t.NumLockers).HasColumnName("NumLockers");
            this.Property(t => t.VOC).HasColumnName("VOC");
            this.Property(t => t.PM10).HasColumnName("PM10");
            this.Property(t => t.CO).HasColumnName("CO");
            this.Property(t => t.NOX).HasColumnName("NOX");
            this.Property(t => t.ReductionInPounds_VOC).HasColumnName("ReductionInPounds_VOC");
            this.Property(t => t.ReductionInPounds_PM10).HasColumnName("ReductionInPounds_PM10");
            this.Property(t => t.ReductionInPounds_CO).HasColumnName("ReductionInPounds_CO");
            this.Property(t => t.ReductionInPounds_NOX).HasColumnName("ReductionInPounds_NOX");
            this.Property(t => t.bikepedconnectivitylocation).HasColumnName("bikepedconnectivitylocation");
            this.Property(t => t.PM2Point5).HasColumnName("PM2Point5");
            this.Property(t => t.ReductionInPounds_PM2Point5).HasColumnName("ReductionInPounds_PM2Point5");
            this.Property(t => t.Users).HasColumnName("Users");
            this.Property(t => t.ExistingUsersScore).HasColumnName("ExistingUsersScore");
            this.Property(t => t.EightPercentOrGreaterGrade).HasColumnName("EightPercentOrGreaterGrade");
            this.Property(t => t.SubstandardRadiiImproved).HasColumnName("SubstandardRadiiImproved");
            this.Property(t => t.SubstandardSightDistanceImproved).HasColumnName("SubstandardSightDistanceImproved");
            this.Property(t => t.NARROWPATHINCREASED).HasColumnName("NARROWPATHINCREASED");
            this.Property(t => t.ExistingCompliantLighting).HasColumnName("ExistingCompliantLighting");
            this.Property(t => t.ProjectCost).HasColumnName("ProjectCost");
            this.Property(t => t.StudiesProjectType).HasColumnName("StudiesProjectType");
            this.Property(t => t.Temp_ResubmitReadyStatus).HasColumnName("Temp_ResubmitReadyStatus");
            this.Property(t => t.NonCOGMoney).HasColumnName("NonCOGMoney");
            this.Property(t => t.PercentOfProjectInEJ).HasColumnName("PercentOfProjectInEJ");
            this.Property(t => t.CommunityBenefits).HasColumnName("CommunityBenefits");
            this.Property(t => t.SupportingEvidence).HasColumnName("SupportingEvidence");
            this.Property(t => t.EJScore).HasColumnName("EJScore");
            this.Property(t => t.AMVHTReduced).HasColumnName("AMVHTReduced");
            this.Property(t => t.PMVHTReduced).HasColumnName("PMVHTReduced");
            this.Property(t => t.DelayReductionScore).HasColumnName("DelayReductionScore");
            this.Property(t => t.SOVPercentageDecrease).HasColumnName("SOVPercentageDecrease");
            this.Property(t => t.MultimodalPotentialScore).HasColumnName("MultimodalPotentialScore");
            this.Property(t => t.MinorMMCTransitOps).HasColumnName("MinorMMCTransitOps");
            this.Property(t => t.MajorMMCTransitOps).HasColumnName("MajorMMCTransitOps");
            this.Property(t => t.MMCWidening).HasColumnName("MMCWidening");
            this.Property(t => t.Mmcbikepriority).HasColumnName("Mmcbikepriority");
            this.Property(t => t.Mmcprotectedcrossing).HasColumnName("Mmcprotectedcrossing");
            this.Property(t => t.Mmctravellane).HasColumnName("Mmctravellane");
            this.Property(t => t.GHGScore).HasColumnName("GHGScore");
            this.Property(t => t.BikePedAddsBikeSpaces).HasColumnName("BikePedAddsBikeSpaces");
            this.Property(t => t.BikePedAddsCoveredBikeSpaces).HasColumnName("BikePedAddsCoveredBikeSpaces");

            // Relationships
            this.HasMany(t => t.TIPProjectEvaluation2010Mode)
                .WithMany(t => t.TIPProjectEvaluation2010)
                .Map(m =>
                    {
                        m.ToTable("TIPProjectEvaluation2010ModeSelected");
                        m.MapLeftKey("ProjectVersionID");
                        m.MapRightKey("ModeID");
                    });


        }
    }
}
