#region INFORMATION
/*======================================================
 * Copyright (c) 2009-2010 DRCOG (www.drcog.org)
 * 
 * DATE		    AUTHOR		    REMARKS
 * 08/05/2009	NKirkes         1. Initial Creation (DTS).
 * 02/01/2010   DDavidson       2. Multiple Improvements.
 * 
 * DESCRIPTION:
 * Interface for base undifferentiated repository for 
 * general data access and lookup collections.
 * ======================================================*/
#endregion

using System;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DRCOG.Domain.Interfaces
{
    public interface IBaseRepository
    {
        T GetLookupSingle<T>(string sprocName, string keyFieldName, IList<SqlParameter> sqlParameters) where T : IConvertible;
        IDictionary<int, string> GetLookupCollection(string sprocName, string keyFieldName, string valueFieldName);
        IDictionary<int, string> GetLookupCollection(string sprocName, string keyFieldName, string valueFieldName, IList<SqlParameter> sqlParameters);
        IDictionary<int, string> GetLookupCollection(string sprocName, string keyFieldName, string valueFieldName, IList<SqlParameter> sqlParameters, IDictionary<string, string> valueAddonValueSeparator);
        IList<string> GetLookupCollection(string sprocName, string valueFieldName);
    }
}
