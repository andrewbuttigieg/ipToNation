using System.Collections.Generic;

public interface IGeoLoc
{
    long IpToLong(string ipAddress);
    List<IpCountry> FromIp(string ipAddress);
}