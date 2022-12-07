using System;

public class Binary
{
    public const string EmptyItem = "0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";

    public static int ToDecimal( string binary )
    {
        return Convert.ToInt32( binary, 2 );
    }

    //
    public static int ToDecimal( string binary, int propertyIndex )
    {
        string snippet = binary.Substring( propertyIndex * 8, 8 );
        return Convert.ToInt32( snippet, 2 );
    }

    public static string ToBinary( int v )
    {
        return Convert.ToString( v, 2 ).PadLeft( 8, '0' );
    }
}