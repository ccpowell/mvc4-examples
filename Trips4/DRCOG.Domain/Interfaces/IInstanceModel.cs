using System;
namespace DRCOG.Domain.Interfaces
{
    public interface IInstance
    {
        int AmendmentStatusId { get; set; }
        bool IsActive { get; set; }
        bool IsTopStatus { get; set; }
        int ProjectVersionId { get; set; }
        //string Year { get; set; }
        Boolean CanDelete(String status);

        bool IsEditable();
    }
}
