using Deadit.Lib.Utility;

namespace Deadit.Lib.Domain.Paging;

public abstract class Pagination
{
    public const uint DEFAULT_PAGE_NUMBER = 1;

    public abstract uint Limit { get; }
    public uint Offset => (Page - 1) * Limit;
    public uint NextPage => Page + 1;
    public uint PreviousPage => !IsFirstPage ? Page - 1 : Page;
    public bool IsFirstPage => Page == 1;
    public string SqlClause => $"LIMIT {Limit} OFFSET {Offset}";

    protected uint _page;

    public uint Page
    {
        get => _page;
        set
        {
            if (value == 0)
            {
                throw new Exception($"Page cannot be 0. Did you mean 1?");
            }

            _page = value;
        }
    }

    public Pagination(uint? page)
    {
        Page = page ?? DEFAULT_PAGE_NUMBER;
    }


    public string GetNextPageUrl(string url)
    {
        Uri uri = new(url);

        return uri.SetQueryParmValue("page", NextPage);
    }


    public string GetPreviousPageUrl(string url)
    {
        Uri uri = new(url);

        return uri.SetQueryParmValue("page", PreviousPage);
    }
}

