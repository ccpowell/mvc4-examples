<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
Inherits="System.Web.Mvc.ViewPage<LocationViewModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Project Location</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="server">
<link href="<%= ResolveUrl("~/Content/SingleView.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.form.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.validate.pack.js")%>" type="text/javascript"></script>
<link href="<%= ResolveUrl("~/Content/slide.css") %>" rel="stylesheet" type="text/css" />
<script src="<%=Page.ResolveClientUrl("~/scripts/slide.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.growing-textarea.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.selectboxes.min.js")%>" type="text/javascript"></script>
<script src="<%=Page.ResolveClientUrl("~/scripts/jquery.popeye-2.0.4.min.js")%>" type="text/javascript"></script>
<link href="<%= Page.ResolveUrl("~/Content/jquery.popeye.css") %>" rel="stylesheet" type="text/css" />
<link href="<%= Page.ResolveUrl("~/Content/jquery.popeye.style.css") %>" rel="stylesheet" type="text/css" />

<script type="text/javascript">
    $(document).ready(function () {
        $(".growable").growing({ buffer: 5 });
            var ppy1options = {
                caption: true,
                navigation: 'hover',
                direction: 'left'
        }

        $('#ppy1').popeye(ppy1options);
    
        // Prevent accidental navigation away
        $(':input', document.dataForm).bind("change", function() { setConfirmUnload(true); });
        $(':input', document.dataForm).bind("keyup", function() { setConfirmUnload(true); });
        $(':button', document.dataForm).unbind("keyup", function() { setConfirmUnload(true); }); // Want to not do this for my hyperlink buttons. -DBD
        //disable the onbeforeunload message if we are using the submitform button
        if ($('#submitForm')) {
            $('#submitForm').click(function() { window.onbeforeunload = null; return true; });
        }
        if ($('#submitImageForm')) {
            $('#submitImageForm').click(function () { window.onbeforeunload = null; return true; });
        }


        //Setup the Ajax form post (allows us to have a nice "Changes Saved" message)
        $("#dataForm").validate({
            submitHandler: function(form) {
                $(form).ajaxSubmit({
                    dataType: 'json',
                    success: function(response) {
                        $('#result').html(response.message);
                        $('#submitForm').addClass('ui-state-disabled');
                    },
                    error: function(XMLHttpRequest, textStatus, errorThrown) {
                        $('#result').text(data.message);
                        $('#result').addClass('error');
                    }
                });
            }
        });

        //Delete location map image
        $('.delete-image').live("click", function () {
            //grab the values from the active form

            var DeleteImageUrl = '<%=Url.Action("DeleteLocationMap","Project" ) %>';
            var recordid = this.id.replace('delete_', '');
            var image = $('#image_' + recordid);


            //alert("Values: registrationId=" + registrationId + "&sponsorName=" + sponsorname + "&sponsorId=" + recordid);
            $.ajax({
                type: "POST",
                url: DeleteImageUrl,
                data: "imageId=" + recordid
                    + "&projectVersionId=<%=Model.TipProjectLocation.ProjectVersionId %>",
                dataType: "json",
                success: function (response) {
                    $('#result-image').html(response.message);

                    $(image).remove();
                    $('.delete-image').remove();
                    $('#upload-image').show();

                    $('div#result-image').addClass('success');
                },
                error: function (response) {
                    $('#result-image').html(response.error);
                }
            });
            return false;
        });



        var top_fullwidth_height = $("#top-fullwidth").height();
//        var uploadwrapperheight = $("#uploadwrapper").height();


        $("#uploadwrapper").css('position', 'absolute');
        $("#uploadwrapper").css('right', '0px');
        $("#uploadwrapper").css('top', top_fullwidth_height + 40);
        //alert(uploadtop + ' ' + uploadtop);

//        var pageheight = $(".page").height();
//        var pagesizeleft = (pageheight - (tabinforightheight + uploadwrapperheight + 40 + 193));

//        if (pagesizeleft < 0) {
//            // need to grow the size of the page
//            $(".page").height(pageheight + 40);
//        }
    });

    function setConfirmUnload(on) {
        $('#submitForm').removeClass('ui-state-disabled');
        $('#result').html("");
        window.onbeforeunload = (on) ? unloadMessage : null;
    }

    function unloadMessage() {
        return 'You have entered new data on this page.  If you navigate away from this page without first saving your data, the changes will be lost.';
    }
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="tab-content-container">
    <% Html.RenderPartial("~/Views/Project/Partials/ProjectGenericPartial.ascx", Model.ProjectSummary); %>
    
    <div class="tab-form-container"> 
        <% using (Html.BeginForm("UpdateLocation", "Project", FormMethod.Post, new { @id = "dataForm" })) %>
        <%{ %>
            <%=Html.ValidationSummary("Unable to update. Please correct the errors and try again.")%>
            <%=Html.Hidden("TipYear", Model.ProjectSummary.TipYear)%>
            <%=Html.Hidden("ProjectVersionId", Model.ProjectSummary.ProjectVersionId)%>
            <%=Html.Hidden("ProjectId", Model.ProjectSummary.ProjectId)%>
            <div id="top-fullwidth" class="box">
                <p>
                    <label>Facility Name:</label>                
                    <%= Html.DrcogTextBox("FacilityName", 
                        Model.ProjectSummary.IsEditable(), 
                        Model.TipProjectLocation.FacilityName, 
                        new { @class = "longInputElement required", title="Please enter a primary road or facility name." })%>
                </p>
                        
                <p><label>Limits (cross streets) or Area of Project Description</label></p>                
                <p>
                    <%= Html.TextArea2("Limits", Model.ProjectSummary.IsEditable(), Model.TipProjectLocation.Limits, 5, 10, new { @class = "longInputElement" }) %>
                    <%--<textarea class="longInputElement" name="Limits"><%=Model.TipProjectLocation.Limits %></textarea>--%>
                </p>
            
            
                <p>
                    <label>Route # (if applicable)</label>                
                </p>

                <div>
                <br />
                <%--<%= Html.DropDownListFor(x => x.TipProjectLocation.CdotRegionId, Model.CDOTRegions, new { @class = "mediumInputElement" })%>--%>

                <p>
                    <h2>Affected CDOT Region</h2>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                        <%= Html.DropDownList("TipProjectLocation.CdotRegionId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.CDOTRegions, "value", "text", Model.TipProjectLocation.CdotRegionId),
                            "-- Select a CDOT Region --",
                            new { @class = "mediumInputElement", title="Please select a CDOT Region." })%>
                    <%}
                      else
                      {%> 
                        <% string value = "None Selected"; if (Model.TipProjectLocation.CdotRegionId != default(int)) { value = Model.CDOTRegions.FirstOrDefault(x => x.Value == Model.TipProjectLocation.CdotRegionId.ToString()).Text; } %>
                        <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                        <%= Html.Hidden("TipProjectLocation.CdotRegionId", Model.TipProjectLocation.CdotRegionId)%>
                        <br />
                    <% } %>
                </p>

                <p>
                    <h2>Affected Project Delays Location</h2>
                    <%if (Model.ProjectSummary.IsEditable())
                      { %>
                        <%= Html.DropDownList("TipProjectLocation.AffectedProjectDelaysLocationId",
                            Model.ProjectSummary.IsEditable(),
                            new SelectList(Model.AffectedProjectDelaysLocation, "value", "text", Model.TipProjectLocation.AffectedProjectDelaysLocationId),
                            "-- Select a Location --",
                            new { @class = "mediumInputElement", title = "Please select an Affected Project Delays Location." })%>
                    <%}
                      else
                      {%> 
                        <% string value = "None Selected"; if (Model.TipProjectLocation.AffectedProjectDelaysLocationId != default(int)) { value = Model.AffectedProjectDelaysLocation.FirstOrDefault(x => x.Value == Model.TipProjectLocation.AffectedProjectDelaysLocationId.ToString()).Text; } %>
                        <span class="fakeinput medium"><%= Html.Encode(value)%></span>
                        <%= Html.Hidden("TipProjectLocation.AffectedProjectDelaysLocationId", Model.TipProjectLocation.AffectedProjectDelaysLocationId)%>
                        <br />
                    <% } %>
                </p>
            </div>
            
                <%if(Model.ProjectSummary.IsEditable()){ %>
                    <div id="submit-container" class="long">
                    <button type="submit" id="submitForm" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all" >Save Changes</button>
                    <div id="result"></div>
                    </div>
                <%} %>
                <div class="clear"></div>
            </div>
            <div>
                <br />
               <h2>Affected Counties</h2>
               <% if(Model.ProjectSummary.IsEditable()) { %>
               <p>Note: This is not versioned. Editing these values will effect all versions of this project</p>
               <% } %>
               <table id="county-shares">
                <tr>
                <td>Primary</td>
                <td>Share</td>
                <td>County</td>
                <%if (Model.ProjectSummary.IsEditable()) { %>
                <td></td>
                <% } %>
                </tr>
               <%foreach (DRCOG.Domain.Models.CountyShareModel cty in Model.CountyShares)
                 { %>                                
                    <tr id="county_row_<%=cty.CountyId.ToString() %>">
                    <td><%= Html.SimpleCheckBox("cty_" + cty.CountyId.ToString() + "_IsPrimary", cty.Primary.Value, cty.ProjectId.ToString() + "_isPrimary", Model.ProjectSummary.IsEditable()  )%></td>
                    <td><span id="cshare_<%= cty.ProjectId.ToString()%>"><%= (int)cty.Share.Value %></span>%</td>
                        <%--<%= Html.DrcogTextBox("cty_" + cty.CountyId.ToString() + "_share", Model.ProjectSummary.IsEditable(), (int)cty.Share.Value, new { style = "width:30px;", @id = "cshare_" + cty.ProjectId.ToString() })%>%
                    </td>--%>
                    <td><%=cty.CountyName %></td>                
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td><button class="delete-county fg-button ui-state-default ui-priority-primary ui-corner-all" name="<%=cty.CountyName %>" id='delete_<%=cty.CountyId.ToString() %>'>Delete</button></td>                                
                        <%} %>            
                    </tr>
               <%} %>
               <%if (Model.ProjectSummary.IsEditable())
                  { %>
                    <tr id="county-editor">
                    <td><%= Html.CheckBox("IsPrimary", Model.ProjectSummary.IsEditable(), false, new { @id ="new_primary" })%></td>
                    <td><%= Html.DrcogTextBox("Share", Model.ProjectSummary.IsEditable(), 0, new { style = "width:30px;", @id = "cshare_new" })%>%</td>
                    <td><%= Html.DropDownList("County",
                                    Model.ProjectSummary.IsEditable(),
                                    new SelectList(Model.AvailableCounties, "key", "value", ""), 
                                    "-- Select a County--",
                                    new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_county" })%></td>
                                    <td><button id="add-county" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button></td>                                
                    </tr>                      
               <%} %>
               <tr>
                    <td>Share Total:</td>
                    <td><div id="county-share-sum"></div></td>
                    <td>&nbsp;</td>
                    <%if (Model.ProjectSummary.IsEditable()) { %>
                    <td>&nbsp;</td>
                    <%} %>
                </tr>
               </table>
            </div>
            <div >
                <br />
               <h2>Affected Municipalities</h2>
               <% if(Model.ProjectSummary.IsEditable()) { %>
               <p>Note: This is not versioned. Editing these values will effect all versions of this project</p>
               <%} %>
               <table id="muni-shares">
                    <tr>
                        <td>Primary</td>
                        <td>Share</td>
                        <td>Municipality</td>
                        <%if (Model.ProjectSummary.IsEditable()) { %>
                        <td></td>
                    <%} %>
                    </tr>
                    <%foreach (DRCOG.Domain.Models.MunicipalityShareModel muni in Model.MuniShares) { %>                                
                    <tr id="muni_row_<%=muni.MunicipalityId.ToString() %>">
                        <td><%= Html.SimpleCheckBox("muni_" + muni.MunicipalityId.ToString() + "_IsPrimary", muni.Primary.Value,   muni.ProjectId.ToString() + "_isPrimary" ,Model.ProjectSummary.IsEditable())%></td>
                        <td><span id="mshare_<%= muni.ProjectId.ToString()%>"><%= (int)muni.Share.Value %></span>%</td>
                        <%--<td><%= Html.DrcogTextBox("muni_" + muni.MunicipalityId.ToString() + "_share", Model.ProjectSummary.IsEditable(), (int)muni.Share.Value, new { style = "width:30px;", @id = "mshare_" + muni.ProjectId.ToString() })%>%</td>--%>
                        <td><%=muni.MunicipalityName%></td>                
                        <%if (Model.ProjectSummary.IsEditable())
                          { %>
                        <td><button class="delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all" name="<%=muni.MunicipalityName%>" id='delete_<%=muni.MunicipalityId.ToString() %>'>Delete</button></td>                                
                        <%} %>            
                    </tr>
                    <%} %>
                    <%if (Model.ProjectSummary.IsEditable()) { %>
                    <tr id="muni-editor">
                        <td><%= Html.CheckBox("IsPrimary", Model.ProjectSummary.IsEditable(), false, new { @id ="new_primary" })%></td>
                        <td><%= Html.DrcogTextBox("Share", Model.ProjectSummary.IsEditable(), 0, new { style = "width:30px;", @id = "mshare_new" })%>%</td>
                        <td><%= Html.DropDownList("Municipality",
                                        Model.ProjectSummary.IsEditable(),
                                        new SelectList(Model.AvailableMunicipalities, "key", "value", ""), 
                                        "-- Select a Municipality--",
                                        new { @class = "mediumInputElement", title = "Please enter a project sponsor agency.", @id="new_muni" })%></td>
                        <td><button disabled='disabled' id="add-muni" class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button></td>                                
                    </tr>                      
                    <%} %>
                    <tr>
                        <td>Share Total:</td>
                        <td><div id="muni-share-sum"></div></td>
                        <td>&nbsp;</td>
                        <%if (Model.ProjectSummary.IsEditable()) { %>
                        <td>&nbsp;</td>
                        <%} %>
                    </tr>
               </table>
            </div>
            
        <%} %>
        <div id="locationmap">
            <span id="uploadplaceholder"></span>
            <div id="uploadwrapper">
                <% using (Html.BeginForm("UpdateImage", "Project", FormMethod.Post, new { id = "imageForm", enctype = "multipart/form-data" })) {%>
                    <%= Html.Hidden("LocationMapId", Model.TipProjectLocation.LocationMapId) %>
                    <%= Html.Hidden("Year", Model.TipProjectLocation.Year)%>
                    <%= Html.Hidden("ProjectVersionId", Model.TipProjectLocation.ProjectVersionId)%>
                    <div id="result-image" style="display:none;"></div>
            
                    <% if (!Model.TipProjectLocation.LocationMapId.Equals(default(int)) && Model.TipProjectLocation.Image != null)
                       { %>
                        <div class="ppy" id="ppy1">
                            <ul class="ppy-imglist">
                                <li>
                                    <a href="<%= Url.Action("ShowPhoto", "Project", new { id = Model.TipProjectLocation.LocationMapId }) %>">
                                        <img src='<%= Url.Action("ShowPhoto", "Project", new { id = Model.TipProjectLocation.LocationMapId }) %>' id="image_<%= Model.TipProjectLocation.LocationMapId %>" class="resize" alt="<%= Model.TipProjectLocation.Image.Name %>" /> 
                                    </a>
                                </li>
                            </ul>
                            <div class="ppy-outer">
                                <div class="ppy-stage-wrap">
                                    <div class="ppy-stage">
                                        <div class="ppy-counter">
                                            <strong class="ppy-current"></strong> / <strong class="ppy-total"></strong> 
                                        </div>
                                    </div>
                                </div>
                                <div class="ppy-nav">
                                    <div class="ppy-nav-wrap">
                                        <a class="ppy-switch-enlarge" title="Enlarge">Enlarge</a>
                                        <a class="ppy-switch-compact" title="Close">Close</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                
                        <%if(Model.ProjectSummary.IsEditable()){ %>
                            <br />
                            <button id="delete_<%= Model.TipProjectLocation.LocationMapId %>" class="delete-image fg-button ui-state-default ui-priority-primary ui-corner-all" >Remove</button>
                        <%} %>
                    <% } %>
                    <%if(Model.ProjectSummary.IsEditable()){ %>
                    <div id="upload-image" <% if (!Model.TipProjectLocation.LocationMapId.Equals(default(int)) && Model.TipProjectLocation.Image != null) { %> style="display:none;" <% } %>>
                        <p>
                            Maximum Upload Size: 4Mb<br />
                        </p>
                        <p><input type="file" id="fileUpload" name="fileUpload" size="23"/></p>
                        <p>
                            <button type="submit" id="submitImageForm" class="fg-button ui-state-default ui-priority-primary ui-corner-all" >Upload</button>
                        </p>
                    </div>
                    <%} %>
                <% } %>
            </div>
        </div>

    </div>
<div class="clear"></div>
</div>






<script type="text/javascript">
    var county_share_total;
    var AddCountyUrl = '<%=Url.Action("AddCountyShare" ) %>';
    var DropCountyUrl = '<%=Url.Action("RemoveCountyShare")%>';
    var AddMuniUrl = '<%=Url.Action("AddMuniShare") %>';
    var DropMuniUrl = '<%=Url.Action("RemoveMuniShare") %>';

    $().ready(function () {

        $('.delete-county').live("click", function () {
            //get the countyId and project id
            var pid = $('#ProjectId').val();
            var ctyId = this.id.replace('delete_', '');
            var name = $(this).attr("name");
            $('#county_row_' + ctyId).empty();
            $.ajax({
                type: "POST",
                url: DropCountyUrl,
                data: "projectId=" + pid + "&countyId=" + ctyId,
                dataType: "json",
                success: function (response) {
                    $('#result').html(response.message);
                    $('#new_county').addOption(ctyId, name).sortOptions();
                    window.onbeforeunload = null;
                    UpdateCountyShareTotal();
                }
            });
            //alert('Need XHR to Remove: county:' + ctyId + ' projectId:' + pid);
            return false;
        });

        $('.delete-muni').live("click", function () {
            //get the countyId and project id
            var pid = $('#ProjectId').val();
            var muniId = this.id.replace('delete_', '');
            var name = $(this).attr("name");
            $('#muni_row_' + muniId).empty();
            $.ajax({
                type: "POST",
                url: DropMuniUrl,
                data: "projectId=" + pid + "&muniId=" + muniId,
                dataType: "json",
                success: function (response) {
                    $('#result').html(response.message);
                    $('#new_muni').addOption(muniId, name).sortOptions();
                    window.onbeforeunload = null;
                    UpdateMuniShareTotal();
                }
            });
            //alert('Need XHR to Remove: muni:' + muniId + ' projectId:' + pid);
            return false;
        });

        UpdateCountyShareTotal();
        UpdateMuniShareTotal();

    });

    function UpdateCountyShareTotal() {
        county_share_total = 0;
        var allocated_share = 0
        var countyEditor = $("tr#county-editor");
        //Sum the shares
        $('input[id^=cshare], td span[id^=cshare]').each(function () {
            var val = $(this).val();
            if (val === '') {
                // try get the text version
                val = $(this).text();
                allocated_share += parseInt(val);
            }
            var parsed = parseInt(val);

            county_share_total += !isNaN(parsed) ? parsed : 0;
        });
        $('#county-share-sum').html(county_share_total);
        //Enable Add button...
        var addButton = $('#add-county');
        //alert(parseInt($('#cshare_new').val()) + " : " + $('#new_county option:selected').val());
        if (county_share_total <= 100 && parseInt($('#cshare_new').val()) > 0 && $('#new_county option:selected').val() != '') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            
            //add disabled class if it does not exist
            if (!addButton.hasClass('ui-state-disabled')) {
                addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
            }
        }

        if (allocated_share == 100) {
            countyEditor.hide();
        } else {
        countyEditor.show();
        }   
    }

    function UpdateMuniShareTotal() {
        var muni_share_total = 0;
        var allocated_share = 0
        var muniEditor = $("tr#muni-editor");

        //Sum the shares
        $('input[id^=mshare], td span[id^=mshare]').each(function () {
            var val = $(this).val();
            if (val === '') {
                // try get the text version
                val = $(this).text();
                allocated_share += parseInt(val);
            }
            var parsed = parseInt(val);

            muni_share_total += !isNaN(parsed) ? parsed : 0;
        });
        $('#muni-share-sum').html(muni_share_total);

        //Enable Add button...
        var addButton = $('#add-muni');
        if (muni_share_total <= 100 && $('#mshare_new').val() != 0 && $('#new_muni option:selected').val() !='') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            //add disabled class if it does not exist
            if (!addButton.hasClass('ui-state-disabled')) {
                addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
            }
        }

        if (allocated_share == 100) {
            muniEditor.hide();
        } else {
            muniEditor.show();
        }
    }
    
    //Hook in the keyup event so we can keep track of changes to the shares
    $('input[id^=cshare]').live('keyup', function () { UpdateCountyShareTotal(); });
    $('input[id^=mshare]').live('keyup', function() { UpdateMuniShareTotal(); });
    $('#new_muni').bind('change', function() { UpdateMuniShareTotal(); });
    $('#new_county').bind('change', function() { UpdateCountyShareTotal(); });
       
    //Add a county to the list
    $('#add-county').click(function () {
        //grab the values from the active form
        var share = $('#cshare_new').val();
        var primary = $('#new_primary').attr('checked');
        var countyId = $('#new_county option:selected').val();
        var countyName = $('#new_county option:selected').text();
        var pid = $('#ProjectId').val();

        //Remove selected option to ensure that we can't re-add it.
        //this leaves a hole, in that you could add, then remove, 
        //then want to re-add, and it would not be in the list.
        $('#new_county option:selected').remove();
        //reset the new share value to 0
        $('#cshare_new').val(0);
        //if primary is checked, see if primary is checked elsewhere, and warn user

        //Do we try to see if the county is already listed?


        primary = (primary === undefined) ? false : primary;

        //alert('Need XHR to add ' + countyName + ' with share ' + share + ' primary ' + primary + ' and CountyId:' + countyId);
        //Add to database via XHR

        $.ajax({
            type: "POST",
            url: AddCountyUrl,
            data: "projectId=" + pid + "&countyId=" + countyId + "&share=" + share + "&isPrimary=" + primary,
            dataType: "json",
            success: function (response) {
                $('#result').html(response.message);
                //Disable the add button
                $('#add-county').addClass('ui-state-disabled').attr('disabled', 'disabled');

                //Add into the DOM
                var content = "<tr id='county_row_" + countyId + "'>";
                if (primary == true) {
                    content += "<td><input id='" + countyId + "_isPrimary' type='checkbox'  name='cty_" + countyId + "_IsPrimary' checked='checked'/></td>";
                } else {
                    content += "<td><input id='" + countyId + "_isPrimary' type='checkbox'  name='cty_" + countyId + "_IsPrimary' /></td>";
                }
                content += "<td><span id='cshare_" + countyId + "'>" + share + "</span>%</td>";
                content += "<td>" + countyName + "</td>";
                content += "<td><button class='delete-county fg-button ui-state-default ui-priority-primary ui-corner-all' name='" + countyName + "' id='delete_" + countyId + "'>Delete</button></td></tr>";
                $('#county-editor').before(content);

                UpdateCountyShareTotal();
                window.onbeforeunload = null;
            }
        });


        return false;
    });

    //Add a county to the list
    $('#add-muni').click(function() {
        //grab the values from the active form
        var share = $('#mshare_new').val();
        var primary = $('#new_primary').attr('checked');
        var muniId = $('#new_muni option:selected').val();
        var muniName = $('#new_muni option:selected').text();
        var pid = $('#ProjectId').val();
        //Remove selected option to ensure that we can't re-add it.
        //this leaves a hole, in that you could add, then remove, 
        //then want to re-add, and it would not be in the list.
        $('#new_muni option:selected').remove();
        //reset the new share value to 0
        $('#mshare_new').val(0);
        //if primary is checked, see if primary is checked elsewhere, and warn user

        primary = (primary === undefined) ? false : primary;

        //Do we try to see if the county is already listed?

        //alert('Need XHR to add muni ' + muniName + ' with share ' + share + ' primary ' + primary + ' and muniId:' + muniId);
        //Add to database via XHR
        $.ajax({
            type: "POST",
            url: AddMuniUrl,
            data: "projectId=" + pid + "&muniId=" + muniId + "&share=" + share + "&isPrimary=" + primary,
            dataType: "json",
            success: function(response) {
                $('#result').html(response.message);
                $('#add-muni').addClass('ui-state-disabled').attr('disabled', 'disabled');

                //Add into the DOM
                var content = "<tr id='muni_row_" + muniId + "'>";
                if (primary == true) {
                    content += "<td><input id='" + muniId + "_isPrimary' type='checkbox'  name='muni_" + muniId + "_IsPrimary' checked='checked'/></td>";
                } else {
                    content += "<td><input id='" + muniId + "_isPrimary' type='checkbox'  name='muni_" + muniId + "_IsPrimary' /></td>";
                }
                content += "<td><span id='mshare_" + muniId + "'>" + share + "</span>%</td>";
                content += "<td>" + muniName + "</td>";
                content += "<td><button class='delete-muni fg-button ui-state-default ui-priority-primary ui-corner-all' name='" + muniName + "' id='delete_" + muniId + "'>Delete</button></td></tr>";
                $('#muni-editor').before(content);

                UpdateMuniShareTotal();
                window.onbeforeunload = null;
            }
        });
        
        return false;
    });
    

</script>
</asp:Content>



