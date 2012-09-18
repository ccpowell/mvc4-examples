using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace OmbudsmanDb
{
    public class OmbudsmanRepository
    {

        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        public OmbudsmanRepository()
        {

        }
        
        public int CreateFacility(global::Ombudsman.Models.Facility facility)
        {
            Logger.Debug("CreateFacility {0}", facility.Name);
            using (var db = new OmbudsmanEntities())
            {
                var added = new Facility()
                {
                    Address1 = facility.Address1,
                    Address2 = facility.Address2,
                    City = facility.City,
                    FacilityTypeId = facility.FacilityTypeId,
                    Fax = facility.Fax,
                    IsActive = facility.IsActive,
                    IsContinuum = facility.IsContinuum,
                    IsMedicaid = facility.IsMedicaid,
                    Name = facility.Name,
                    NumberOfBeds = facility.NumberOfBeds,
                    OmbudsmanId = facility.OmbudsmanId,
                    Phone = facility.Phone,
                    State = facility.State,
                    ZipCode = facility.ZipCode
                };
                db.AddToFacilities(added);
                db.SaveChanges();
                return added.FacilityId;
            }
        }

        private string GetOmbudsmanNameIf(Ombudsman ombudsman)
        {
            if (ombudsman == null)
            {
                return null;
            }
            else
            {
                return ombudsman.Name;
            }
        }
        /// <summary>
        /// Return a list of all facilities.
        /// </summary>
        /// <remarks>No filtering is performed since this is a small (lt 1000 ) set of items.</remarks>
        /// <returns>listing of facilities</returns>
        public List<global::Ombudsman.Models.Facility> GetFacilities()
        {
            var facilities = new List<global::Ombudsman.Models.Facility>();
            using (var db = new OmbudsmanEntities())
            {
                // we need to fetch all of them to get the Ombudsman Name
                var all = db.Facilities.AsEnumerable();
                facilities.AddRange(all.Select(facility => new global::Ombudsman.Models.Facility()
                {
                    Address1 = facility.Address1,
                    Address2 = facility.Address2,
                    City = facility.City,
                    FacilityId = facility.FacilityId,
                    FacilityTypeId = facility.FacilityTypeId,
                    FacilityTypeName = facility.FacilityType.Name,
                    Fax = facility.Fax,
                    IsActive = facility.IsActive,
                    IsContinuum = facility.IsContinuum,
                    IsMedicaid = facility.IsMedicaid,
                    Name = facility.Name,
                    NumberOfBeds = facility.NumberOfBeds,
                    OmbudsmanId = facility.OmbudsmanId,
                    OmbudsmanName = GetOmbudsmanNameIf(facility.Ombudsman),
                    Phone = facility.Phone,
                    State = facility.State,
                    ZipCode = facility.ZipCode
                }));
            }
            return facilities;
        }

        public global::Ombudsman.Models.Facility GetFacility(int id)
        {
            using (var db = new OmbudsmanEntities())
            {
                var facility = db.Facilities.Where(f => f.FacilityId == id).Single();
                var found = new global::Ombudsman.Models.Facility()
                {
                    Address1 = facility.Address1,
                    Address2 = facility.Address2,
                    City = facility.City,
                    FacilityId = facility.FacilityId,
                    FacilityTypeId = facility.FacilityTypeId,
                    FacilityTypeName = facility.FacilityType.Name,
                    Fax = facility.Fax,
                    IsActive = facility.IsActive,
                    IsContinuum = facility.IsContinuum,
                    IsMedicaid = facility.IsMedicaid,
                    Name = facility.Name,
                    NumberOfBeds = facility.NumberOfBeds,
                    OmbudsmanId = facility.OmbudsmanId,
                    OmbudsmanName = GetOmbudsmanNameIf(facility.Ombudsman),
                    Phone = facility.Phone,
                    State = facility.State,
                    ZipCode = facility.ZipCode
                };
                return found;
            }
        }

        /// <summary>
        /// Get a listing of all of the Ombudsman. 
        /// </summary>
        /// <remarks>No filtering is performed since this is a small (lt 1000 ) set of items.</remarks>
        /// <returns>listing of ombudsmen</returns>
        public List<global::Ombudsman.Models.Ombudsman> GetOmbudsmen()
        {
            var ombudsmen = new List<global::Ombudsman.Models.Ombudsman>();
            using (var db = new OmbudsmanEntities())
            {
               ombudsmen.AddRange(db.Ombudsmen.Select(omb => new global::Ombudsman.Models.Ombudsman()
               {
                   Address1 = omb.Address1,
                   Address2 = omb.Address2,
                   City = omb.City,
                   Fax = omb.Fax,
                   Name = omb.Name,
                   OmbudsmanId = omb.OmbudsmanId,
                   Phone = omb.Phone,
                   State = omb.State,
                   UserName = omb.UserName,
                   ZipCode = omb.ZipCode
               }));
            }
            return ombudsmen;
        }

        public global::Ombudsman.Models.Ombudsman GetOmbudsman(int id)
        {
            using (var db = new OmbudsmanEntities())
            {
                var ombudsman = db.Ombudsmen.FirstOrDefault(o => o.OmbudsmanId == id);
                if (ombudsman == null)
                {
                    return null;
                }
                var found = new global::Ombudsman.Models.Ombudsman()
                {
                    Email = ombudsman.Email,
                    Address1 = ombudsman.Address1,
                    Address2 = ombudsman.Address2,
                    City = ombudsman.City,
                    Fax = ombudsman.Fax,
                    Name = ombudsman.Name,
                    OmbudsmanId = ombudsman.OmbudsmanId,
                    Phone = ombudsman.Phone,
                    State = ombudsman.State,
                    UserName = ombudsman.UserName,
                    ZipCode = ombudsman.ZipCode
                };
                return found;
            }
        }

        /// <summary>
        /// Get a listing of all Facilities for an Ombudsmen.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<global::Ombudsman.Models.Facility> GetFacilitiesForOmbudsman(int id)
        {
            var facilities = new List<global::Ombudsman.Models.Facility>();
            using (var db = new OmbudsmanEntities())
            {
                // we need to fetch all of them to get the Ombudsman Name
                var all = db.Facilities.Where(f => f.OmbudsmanId == id).AsEnumerable();
                facilities.AddRange(all.Select(facility => new global::Ombudsman.Models.Facility()
                {
                    Address1 = facility.Address1,
                    Address2 = facility.Address2,
                    City = facility.City,
                    FacilityId = facility.FacilityId,
                    FacilityTypeId = facility.FacilityTypeId,
                    FacilityTypeName = facility.FacilityType.Name,
                    Fax = facility.Fax,
                    IsActive = facility.IsActive,
                    IsContinuum = facility.IsContinuum,
                    IsMedicaid = facility.IsMedicaid,
                    Name = facility.Name,
                    NumberOfBeds = facility.NumberOfBeds,
                    OmbudsmanId = facility.OmbudsmanId,
                    OmbudsmanName = GetOmbudsmanNameIf(facility.Ombudsman),
                    Phone = facility.Phone,
                    State = facility.State,
                    ZipCode = facility.ZipCode
                }));
            }
            return facilities;
        }

        public List<global::Ombudsman.Models.FacilityType> GetFacilityTypes()
        {
            var types = new List<global::Ombudsman.Models.FacilityType>();
            using (var db = new OmbudsmanEntities())
            {
                // we need to fetch all of them to get the Ombudsman Name
                types.AddRange(db.FacilityTypes.Select(ft => new global::Ombudsman.Models.FacilityType()
                {
                    FacilityTypeId = ft.FacilityTypeId,
                    Name = ft.Name
                }));
            }
            return types;
        }

        public void UpdateFacility(global::Ombudsman.Models.Facility facility)
        {
            using (var db = new OmbudsmanEntities())
            {
                var found = db.Facilities.Single(f => f.FacilityId == facility.FacilityId);
                found.Address1 = facility.Address1;
                found.Address2 = facility.Address2;
                found.City = facility.City;
                found.FacilityTypeId = facility.FacilityTypeId;
                found.Fax = facility.Fax;
                found.IsActive = facility.IsActive;
                found.IsContinuum = facility.IsContinuum;
                found.IsMedicaid = facility.IsMedicaid;
                found.Name = facility.Name;
                found.NumberOfBeds = facility.NumberOfBeds;
                found.OmbudsmanId = facility.OmbudsmanId;
                found.Phone = facility.Phone;
                found.State = facility.State;
                found.ZipCode = facility.ZipCode;
                db.SaveChanges();
            }
        }

        public int CreateOmbudsman(global::Ombudsman.Models.Ombudsman ombudsman)
        {
            using (var db = new OmbudsmanEntities())
            {
                var added = new Ombudsman()
                {
                    Email = ombudsman.Email,
                    Address1 = ombudsman.Address1,
                    Address2 = ombudsman.Address2,
                    City = ombudsman.City,
                    Fax = ombudsman.Fax,
                    Name = ombudsman.Name,
                    Phone = ombudsman.Phone,
                    State = ombudsman.State,
                    UserName = ombudsman.UserName,
                    ZipCode = ombudsman.ZipCode
                };
                db.AddToOmbudsmen(added);
                db.SaveChanges();
                return added.OmbudsmanId;
            }
        }

        /// <summary>
        /// Update ombudsman. ID and UserName cannot be updated.
        /// </summary>
        /// <param name="ombudsman"></param>
        public void UpdateOmbudsman(global::Ombudsman.Models.Ombudsman ombudsman)
        {
            using (var db = new OmbudsmanEntities())
            {
                var found = db.Ombudsmen.Single(o => o.OmbudsmanId == ombudsman.OmbudsmanId);
                found.Email = ombudsman.Email;
                found.Address1 = ombudsman.Address1;
                found.Address2 = ombudsman.Address2;
                found.City = ombudsman.City;
                found.Fax = ombudsman.Fax;
                found.Name = ombudsman.Name;
                found.Phone = ombudsman.Phone;
                found.State = ombudsman.State;
                found.ZipCode = ombudsman.ZipCode;
                db.SaveChanges();
            }
        }

        public int GetOmbudsmanIdFromName(string name)
        {
            using (var db = new OmbudsmanEntities())
            {
                var found = db.Ombudsmen.SingleOrDefault(o => o.Name == name);
                if (found != null)
                {
                    return found.OmbudsmanId;
                }
                return 0;
            }
        }

        public int GetOmbudsmanIdFromUserName(string name)
        {
            using (var db = new OmbudsmanEntities())
            {
                var found = db.Ombudsmen.SingleOrDefault(o => o.UserName == name);
                if (found != null)
                {
                    return found.OmbudsmanId;
                }
                return 0;
            }
        }
    }
}
