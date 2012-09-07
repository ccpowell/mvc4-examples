using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Ef5Contacts.Models.Mapping;

namespace Ef5Contacts.Models
{
    public class TRIPS_UserContext : DbContext
    {
        static TRIPS_UserContext()
        {
            Database.SetInitializer<TRIPS_UserContext>(null);
        }

		public TRIPS_UserContext()
			: base("Name=TRIPS_UserContext")
		{
		}

        public DbSet<aspnet_Applications> aspnet_Applications { get; set; }
        public DbSet<aspnet_Membership> aspnet_Membership { get; set; }
        public DbSet<aspnet_Paths> aspnet_Paths { get; set; }
        public DbSet<aspnet_PersonalizationAllUsers> aspnet_PersonalizationAllUsers { get; set; }
        public DbSet<aspnet_PersonalizationPerUser> aspnet_PersonalizationPerUser { get; set; }
        public DbSet<aspnet_Profile> aspnet_Profile { get; set; }
        public DbSet<aspnet_Roles> aspnet_Roles { get; set; }
        public DbSet<aspnet_SchemaVersions> aspnet_SchemaVersions { get; set; }
        public DbSet<aspnet_Users> aspnet_Users { get; set; }
        public DbSet<aspnet_WebEvent_Events> aspnet_WebEvent_Events { get; set; }
        public DbSet<ContactList> ContactLists { get; set; }
        public DbSet<ContactList_Attribute> ContactList_Attribute { get; set; }
        public DbSet<ContactList_AttributeType> ContactList_AttributeType { get; set; }
        public DbSet<ContactList_ListAttribute> ContactList_ListAttribute { get; set; }
        public DbSet<ContactList_Member> ContactList_Member { get; set; }
        public DbSet<ContactList_MemberAudit> ContactList_MemberAudit { get; set; }
        public DbSet<ContactList_Owner> ContactList_Owner { get; set; }
        public DbSet<ContactList_Status> ContactList_Status { get; set; }
        public DbSet<ContactList_UserCache> ContactList_UserCache { get; set; }
        public DbSet<ProfileProperty> ProfileProperties { get; set; }
        public DbSet<ProfilePropertyValue> ProfilePropertyValues { get; set; }
        public DbSet<StatusType> StatusTypes { get; set; }
        public DbSet<UserVersion> UserVersions { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<vw_aspnet_Applications> vw_aspnet_Applications { get; set; }
        public DbSet<vw_aspnet_MembershipUsers> vw_aspnet_MembershipUsers { get; set; }
        public DbSet<vw_aspnet_Profiles> vw_aspnet_Profiles { get; set; }
        public DbSet<vw_aspnet_Roles> vw_aspnet_Roles { get; set; }
        public DbSet<vw_aspnet_Users> vw_aspnet_Users { get; set; }
        public DbSet<vw_aspnet_UsersInRoles> vw_aspnet_UsersInRoles { get; set; }
        public DbSet<vw_aspnet_WebPartState_Paths> vw_aspnet_WebPartState_Paths { get; set; }
        public DbSet<vw_aspnet_WebPartState_Shared> vw_aspnet_WebPartState_Shared { get; set; }
        public DbSet<vw_aspnet_WebPartState_User> vw_aspnet_WebPartState_User { get; set; }
        public DbSet<vw_Membership_GetMembershipUsers> vw_Membership_GetMembershipUsers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new aspnet_ApplicationsMap());
            modelBuilder.Configurations.Add(new aspnet_MembershipMap());
            modelBuilder.Configurations.Add(new aspnet_PathsMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationAllUsersMap());
            modelBuilder.Configurations.Add(new aspnet_PersonalizationPerUserMap());
            modelBuilder.Configurations.Add(new aspnet_ProfileMap());
            modelBuilder.Configurations.Add(new aspnet_RolesMap());
            modelBuilder.Configurations.Add(new aspnet_SchemaVersionsMap());
            modelBuilder.Configurations.Add(new aspnet_UsersMap());
            modelBuilder.Configurations.Add(new aspnet_WebEvent_EventsMap());
            modelBuilder.Configurations.Add(new ContactListMap());
            modelBuilder.Configurations.Add(new ContactList_AttributeMap());
            modelBuilder.Configurations.Add(new ContactList_AttributeTypeMap());
            modelBuilder.Configurations.Add(new ContactList_ListAttributeMap());
            modelBuilder.Configurations.Add(new ContactList_MemberMap());
            modelBuilder.Configurations.Add(new ContactList_MemberAuditMap());
            modelBuilder.Configurations.Add(new ContactList_OwnerMap());
            modelBuilder.Configurations.Add(new ContactList_StatusMap());
            modelBuilder.Configurations.Add(new ContactList_UserCacheMap());
            modelBuilder.Configurations.Add(new ProfilePropertyMap());
            modelBuilder.Configurations.Add(new ProfilePropertyValueMap());
            modelBuilder.Configurations.Add(new StatusTypeMap());
            modelBuilder.Configurations.Add(new UserVersionMap());
            modelBuilder.Configurations.Add(new VersionMap());
            modelBuilder.Configurations.Add(new vw_aspnet_ApplicationsMap());
            modelBuilder.Configurations.Add(new vw_aspnet_MembershipUsersMap());
            modelBuilder.Configurations.Add(new vw_aspnet_ProfilesMap());
            modelBuilder.Configurations.Add(new vw_aspnet_RolesMap());
            modelBuilder.Configurations.Add(new vw_aspnet_UsersMap());
            modelBuilder.Configurations.Add(new vw_aspnet_UsersInRolesMap());
            modelBuilder.Configurations.Add(new vw_aspnet_WebPartState_PathsMap());
            modelBuilder.Configurations.Add(new vw_aspnet_WebPartState_SharedMap());
            modelBuilder.Configurations.Add(new vw_aspnet_WebPartState_UserMap());
            modelBuilder.Configurations.Add(new vw_Membership_GetMembershipUsersMap());
        }
    }
}
