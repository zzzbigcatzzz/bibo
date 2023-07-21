using System;


public class RouteListQuestionLienQuan
{
    public int status { set; get; }
    public List<LsQuestion> message { set; get; }
}

public class RouteDetailQuestion
{
    public int status { set; get; }
    public objDetailQuestion message { set; get; }
}
public class objDetailQuestion
{
    public LsQuestion LsQuestion { set; get; }
    public List<lsAnswer> lsAnswer { set; get; }
}

public class lsAnswer
{
    public string RowId { set; get; }
    public string Content { set; get; }
    public string LinkImageAuthor { set; get; }
    public string AuthorName { set; get; }
    public int TotalComment { set; get; }
    public int TotalLike { set; get; }
    public DateTime createdDate { set; get; }
}

public class RouteListCate
{
    public int status { set; get; }
    public List<objCombox> message { set; get; }
}

public class RouteListQuestion
{
    public int status { set; get; }
    public objQuestion message { set; get; }
    public metadata metadata { set; get; }
}
public class objQuestion
{
    public List<LsQuestion> LsQuestion { set; get; }
    public List<objCombox> lsCate { set; get; }
}

public class LsQuestion
{
    public string RowId { set; get; }
    public string Content { set; get; }
    public string LinkImageAuthor { set; get; }
    public string AuthorName { set; get; }
    public int TotalComment { set; get; }
    public int TotalLike { set; get; }
    public int TotalShare { set; get; }
    public List<objCombox> lsCate { set; get; }
    public DateTime createdDate { set; get; }
}

public class ItemQuestion
{
    public string RowId { set; get; }
    public string Content { set; get; }
    public string LinkImageAuthor { set; get; }
    public string AuthorName { set; get; }
    public int TotalComment { set; get; }
    public int TotalLike { set; get; }
    public int TotalShare { set; get; }
    public string CateCode { set; get; }
    public string CateName { set; get; }
    public DateTime createdDate { set; get; }
    public int TotalRows { set; get; }
    public int NumOfPages { set; get; }
}

public class ObjGuiCauHoi
{
    public string content { set; get; }
    public string LinkImageUser { set; get; }
    public string createdBy { set; get; }
    public string createdByName { set; get; }
    public List<string> cateId { set; get; }
}



public class RouteDetailPost
{
    public int status { set; get; }
    public DetailPost message { set; get; }
}

public class DetailPost
{
    public int RowId { set; get; }
    public string Title { set; get; }
    public string LinkImage { set; get; }
    public string Content { set; get; }
    public DateTime createDate { set; get; }
    public int TotalLike { set; get; }
    public int TotalComment { set; get; }
}


public class RouteListPost
{
    public int status { set; get; }
    public List<ListPost> message { set; get; }
    public metadata metadata { set; get; }
}
public class ListPost
{
    public string RowId { set; get; }
    public string Title { set; get; }
    public string LinkImage { set; get; }
    public string LinkImageAuthor { set; get; }
    public string AuthorName { set; get; }
    public string experience { set; get; }
    public int TotalComment { set; get; }
    public int TotalLike { set; get; }
    public int cate_id { set; get; }
    public DateTime createdDate { set; get; }
    public int TotalRows { set; get; }
    public int NumOfPages { set; get; }
}

public class RouteDetailDoctor
{
    public int status { set; get; }
    public DetailDoctor message { set; get; }
}

public class DetailDoctor
{
    public int RowId { set; get; }
    public string FullName { set; get; }
    public string LinkImage { set; get; }
    public string Position { set; get; }
    public string Introduce { set; get; }
    public int NumberLike { set; get; }
    public int NumberPost { set; get; }
}

public class RouteInfoDoctor
{
    public int status { set; get; }
    public objMessage message { set; get; }
    public metadata metadata { set; get; }
}
public class objMessage
{
    public List<ItemDoctor> ItemDoctor { set; get; }
    public List<objCombox> lsCate { set; get; }
}

public class ItemDoctor
{
    public int RowId { set; get; }
    public string FullName { set; get; }
    public string LinkImage { set; get; }
    public string Position { set; get; }
    public int NumberLike { set; get; }
    public List<objCombox> lsCate { set; get; }
}
public class LsDoctor
{
    public int RowId { set; get; }
    public string FullName { set; get; }
    public string LinkImage { set; get; }
    public string Position { set; get; }
    public int NumberLike { set; get; }
    public string CateCode { set; get; }
    public string CateName { set; get; }
    public int TotalRows { set; get; }
    public int NumOfPages { set; get; }
}

public class metadata
{
    public int current_page { get; set; }
    public int per_page { get; set; }
    public int total_item { get; set; }
    public int total_page { get; set; }
}