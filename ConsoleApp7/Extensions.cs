using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp7
{
    public static class Extensions
    {
		// ValueConverter<ModelClrType, DbClrType>( 
		// {
		//		model => (from model to db conversion),
		//		db => (from db to model conversion)
		// )};
		
		public static ValueConverter<int, long> IntAndLongConverter()
		{
			return new ValueConverter<int, long>(
				v => (long)v,
				v => (int)v
			);
		}

		//public static ValueConverter<long, int> IntAndLongConverter2()
		//{
		//	return new ValueConverter<long, int>(
		//		v => (int)v,
		//		v => (long)v,
		//		new ConverterMappingHints(valueGeneratorFactory: (p, t) => new TemporaryBigIntValueGenerator())
		//	);
		//}

		public static ValueConverter<int, long> IntAndLongConverter2()
		{
			return new ValueConverter<int, long>(
				v => Convert.ToInt64(v.ToString()),
				v => Convert.ToInt32(v.ToString())
			);
		}

	}
}
