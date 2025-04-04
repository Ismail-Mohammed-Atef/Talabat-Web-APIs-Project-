﻿namespace Talabat.APIS.Helper
{
	public class Pagination<T>
	{
		public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

		public Pagination(int pageIndex,int pageSize,int count , IReadOnlyList<T> result)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			Count = count;
			Data = result;
		}

    }
}
