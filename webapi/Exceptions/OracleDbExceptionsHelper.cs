using System.Data;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace MovieCatalogBackend.Exceptions;

public class OracleDbExceptionsHelper : IDbExceptionsHelper
{
    public string Message { get; private set; }

    public bool IsAlreadyExist(Exception e)
    {
        if (e is not OracleException { Number: 1 } && e.InnerException is not OracleException { Number: 1 })
            return false;
        Message = "Already exists";
        return true;
    }

    public bool IsNotFound(Exception e)
    {
        if (e is OracleException { Number: 2291 } || e.InnerException is  OracleException { Number: 2291 })
            Message = "Some relation couldn't be found";
        else if (e is DBConcurrencyException || e.InnerException is DbUpdateConcurrencyException)
            Message = "Seemed like entity couldn't be found";
        else return false;
        return true;
    }
}