using System;

public class RouteVideoCourse
{
    public int status { set; get; }
    public List<lsContentCourse> message { set; get; }
}
public class lsContentCourse
{
    public string RowId { set; get; }
    public string Title { set; get; }
    public int TotalVideo { set; get; }
    public List<ItemVideo> LsItemVideo{ set; get; }
}
public class ItemVideo
{
    public string TitleVideo { set; get; }
    public string LinkVideo { set; get; }
    public string TimeVideo { set; get; }
}

public class ResVideoCourse
{
    public string RowId { set; get; }
    public string Title { set; get; }
    public string TitleVideo { set; get; }
    public string LinkVideo { set; get; }
    public string TimeVideo { set; get; }
}

public class RouteDetailCourse
{
    public int status { set; get; }
    public DetailCourse message { set; get; }
}
public class DetailCourse
{
    public string RowId { set; get; }
    public string Title { set; get; }
    public string LinkImage { set; get; }
    public string LinkImageAuthor { set; get; }
    public string AuthorName { set; get; }
    public string experience { set; get; }
    public int TotalComment { set; get; }
    public int TotalLike { set; get; }
    public DateTime createdDate { set; get; }
    public string Content { set; get; }
    public string Position { set; get; }
    public string GioiThieu { set; get; }
    public string QTCongTac { set; get; }
    public int favourite { set; get; }
}

public class RouteListCourse
{
    public int status { set; get; }
    public List<ListCourse> message { set; get; }
    public metadata metadata { set; get; }
}
public class ListCourse
{
    public string RowId { set; get; }
    public string Title { set; get; }
    public string LinkImage { set; get; }
    public string LinkImageAuthor { set; get; }
    public string AuthorName { set; get; }
    public string experience { set; get; }
    public int TotalComment { set; get; }
    public int TotalLike { set; get; }
    public DateTime createdDate { set; get; }
    public int TotalRows { set; get; }
    public int NumOfPages { set; get; }
}
