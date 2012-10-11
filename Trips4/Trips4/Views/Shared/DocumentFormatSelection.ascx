<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.String>" %>
<div class="report-format-list">
    <span>Report Format</span>
    <label>
        <input type="radio" value="PDF" name="<%=Model%>" checked="checked" />
        PDF
    </label>
    <label>
        <input type="radio" value="Excel" name="<%=Model%>" />
        Excel
    </label>
    <label>
        <input type="radio" value="Word" name="<%=Model%>" />
        Word
    </label>
</div>
