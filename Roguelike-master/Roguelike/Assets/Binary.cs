using System;

public class Binary
{
    public static int ToDecimal( string binary )
    {
        return Convert.ToInt32( binary, 2 );
    }

    //
    public static int ToDecimal( string binary, int propertyIndex, int length = 8 )
    {
        string snippet = binary.Substring( propertyIndex * 8, length );
        return Convert.ToInt32( snippet, 2 );
    }

    public static string ToBinary( int v )
    {
        return Convert.ToString( v, 2 );
    }
}