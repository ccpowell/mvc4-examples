<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<DRCOG.Domain.Models.TIPProject.PoolProject>>" %>

    <div >
       <h2>Pool Projects</h2>
       <table id="pool-project">
        <tr>
        <th>Project Name</th>
        <th>Description</th>
        <th>Begin</th>
        <th>End</th>
        <th>Cost</th>
        <th></th>
        </tr>

    <% foreach (var item in Model.ToList<DRCOG.Domain.Models.TIPProject.PoolProject>()) { %>
        
        <tr id="pool_row_<%=item.PoolProjectID.ToString() %>">
            <td><%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_ProjectName", true, item.ProjectName.ToString(), new { style = "width:200px;", @id = "poolproject_" + item.PoolProjectID.ToString(), @maxlength = "255" })%></td>
            <td><%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_Description", true, item.Description.ToString(), new { style = "width:150px;", @maxlength = "75" })%></td>
            <td><%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_BeginAt", true, item.BeginAt.ToString(), new { style = "width:100px;", @maxlength = "75" })%></td>
            <td><%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_EndAt", true, item.EndAt.ToString(), new { style = "width:100px;", @maxlength = "75" })%></td>
            <td><%= Html.DrcogTextBox("poolproject_" + item.PoolProjectID.ToString() + "_Cost", true, String.Format("{0:F}", item.Cost), new { style = "width:100px;", @maxlength = "75", @alt = "money" })%></td>
            <td><button class="delete-pool fg-button ui-state-default ui-priority-primary ui-corner-all" id='delete_<%=item.PoolProjectID.ToString() %>'>Delete</button></td>
        </tr>
        
    <% } %>
        <tr id="pool-editor">
            <td><%= Html.DrcogTextBox("ProjectName", true, "", new { style = "width: 200px;", @id = "new_poolname", @maxlength = "255" })%></td>
            <td><%= Html.DrcogTextBox("Description", true, "", new { style = "width: 150px;", @id = "new_pooldesc", @maxlength = "75" })%></td>
            <td><%= Html.DrcogTextBox("BeginAt", true, "", new { style = "width: 100px;", @id = "new_poolbeginat", @maxlength = "75" })%></td>
            <td><%= Html.DrcogTextBox("EndAt", true, "", new { style = "width: 100px;", @id = "new_poolendat", @maxlength = "75" })%></td>
            <td><%= Html.DrcogTextBox("Cost", true, "", new { style = "width: 100px;", @id = "new_poolcost", @maxlength = "75", @alt = "money" })%></td>
            <td><button id="add-pool" disabled='disabled' class="fg-button ui-state-default ui-priority-primary ui-state-disabled ui-corner-all">Add</button></td>
        </tr>

       </table>
    </div>


<script type="text/javascript">
    var county_share_total;
    var AddPoolUrl = '<%=Url.Action("AddPoolProject" ) %>';
    var DropPoolUrl = '<%=Url.Action("DeletePoolProject")%>';
    var EditPoolUrl = '<%=Url.Action("UpdatePoolProject")%>';


    $().ready(function() {
        $('.delete-pool').live("click", function() {
            //get the poolprojectid : pool_row_5022
            var poolid = this.id.replace('delete_', '');
            var row = $('#pool_row_' + poolid);
            //$('#pool_row_' + poolid).empty();
            $.ajax({
                type: "POST",
                url: DropPoolUrl,
                data: "poolProjectId=" + poolid,
                dataType: "json",
                success: function(response) {
                    $('#result').html(response.message);
                    //var div = $('#pool_div_' + poolid);
                    $(row).empty();
                    $('div#result').addClass('success');
                    autoHide();
                }
            });
            return false;
        });

        //Update a pool in the list
        $('.update-pool').live("click", function() {
            var poolprojectid = this.id.replace('delete_', '');
            //grab the values from the active form
            var poolname = $('#poolproject_' + poolprojectid).val();
            var pooldesc = $('#poolproject_' + poolprojectid + '_Description').val();
            var poolbeginat = $('#poolproject_' + poolprojectid + '_BeginAt').val();
            var poolendat = $('#poolproject_' + poolprojectid + '_EndAt').val();
            var poolcost = $('#poolproject_' + poolprojectid + '_Cost').val();

            //Do we try to see if the pool name is already listed?

            //Add to database via XHR
            //alert('Need XHR Big Test to Add: poolProjectId=' + poolprojectid + '&projectName=' + poolname + '&description=' + pooldesc + '&beginAt=' + poolbeginat + '&endAt=' + poolendat + '&cost=' + poolcost);
            $.ajax({
                type: "POST",
                url: EditPoolUrl,
                data: "poolProjectId=" + poolprojectid + "&projectName=" + poolname + "&description=" + pooldesc + "&beginAt=" + poolbeginat + "&endAt=" + poolendat + "&cost=" + poolcost,
                dataType: "json",
                success: function(response) {
                    $('#result').html(response.message);

                    //Disable the add button
                    $('.update-pool').html("Delete").addClass('delete-pool').removeClass('update-pool');
                    $('div#result').addClass('success');
                    autoHide();
                }
            });

            return false;
        });

        //UpdatePoolProjectTotal();

    });

    //Hook in the keyup event so we can keep track of changes to the shares
//    $('input[id^=cshare]').live('keyup', function() { UpdatePoolProjectTotal(); });
    //$('#new_pool').bind('change', function() { UpdatePoolProjectTotal(); });
    $('input[id^=poolproject]').live('keyup', function() { ChangePoolProjectToUpdate(); });
    $('input[id^=new_pool]').live('keyup', function() { UpdatePoolProjectTotal(); });

    //Add a county to the list
    $('#add-pool').click(function() {
        //grab the values from the active form
        var poolname = $('#new_poolname').val();
        var pooldesc = $('#new_pooldesc').val();
        var poolbeginat = $('#new_poolbeginat').val();
        var poolendat = $('#new_poolendat').val();
        var poolcost = $('#new_poolcost').val().replace(/[,]/g, "");
        var poolprojectid;
        var poolmasterid = $('#TipProjectScope_ProjectVersionId').val();

        //reset the new share value to 0
        $('#new_poolname').val("");
        $('#new_pooldesc').val("");
        $('#new_poolbeginat').val("");
        $('#new_poolendat').val("");
        $('#new_poolcost').val("");

        //Do we try to see if the pool name is already listed?

        //Add to database via XHR

        //alert('Need XHR Big Test to Add: poolMasterVersionId=' + poolmasterid + '&projectName=' + poolname + '&description=' + pooldesc + '&beginAt=' + poolbeginat + '&endAt=' + poolendat + '&cost=' + poolcost);

        $.ajax({
            type: "POST",
            url: AddPoolUrl,
            data: "poolMasterVersionId=" + poolmasterid + "&projectName=" + poolname + "&description=" + pooldesc + "&beginAt=" + poolbeginat + "&endAt=" + poolendat + "&cost=" + poolcost,
            dataType: "json",
            success: function(response) {
                $('#result').html(response.message);
                //Add into the DOM
                poolprojectid = response.poolprojectid;
                var content = "<tr id='pool_row_" + poolprojectid + "'>";
                content += "<td><input id='poolproject_" + poolprojectid + "' type='text' value='" + poolname + "' style='width: 200px;' name='poolproject_" + poolprojectid + "_ProjectName' maxlength = 255/></td>";
                content += "<td><input type='text' value='" + pooldesc + "' style='width: 150px;' name='poolproject_" + poolprojectid + "_Description' maxlength = 75/></td>";
                content += "<td><input type='text' value='" + poolbeginat + "' style='width: 100px;' name='poolproject_" + poolprojectid + "_BeginAt' maxlength = 75/></td>";
                content += "<td><input type='text' value='" + poolendat + "' style='width: 100px;' name='poolproject_" + poolprojectid + "_EndAt' maxlength = 75/></td>";
                content += "<td><input type='text' value='" + poolcost + "' style='width: 100px;' name='poolproject_" + poolprojectid + "_Cost' maxlength = 75 alt='money'/></td>";
                content += "<td><button class='delete-pool fg-button ui-state-default ui-priority-primary ui-corner-all' id='delete_" + poolprojectid + "'>Delete</button></td></tr>";
                $('#pool-editor').before(content);

                //Disable the add button
                $('#add-pool').addClass('ui-state-disabled').attr('disabled', 'disabled');
                $('div#result').addClass('success');

                autoHide();
            }
        });
        return false;
    });

    function autoHide() {
            //$('div#result:contains("error")').addClass('error');
        setTimeout(function() {
            $("div#result").fadeOut("slow", function() {
                $("div#result").empty().removeClass().removeAttr('style');
            });
        }, 5000);
    }

    function ChangePoolProjectToUpdate() {
        $('.delete-pool').html("Update").removeClass('delete-pool').addClass('update-pool');
    }

    function UpdatePoolProjectTotal() {
        county_share_total = 0;
        //Sum the shares
        /*
        $('input[id^=cshare]').each(function() {
            county_share_total += parseInt(this.value);
        });
        $('#county-share-sum').html(county_share_total);
        */
        //Enable Add button...
        var addButton = $('#add-pool');
        //if ($('new_poolname').val() != '') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        //}
        /*
        if (county_share_total == 100 && parseInt($('#cshare_new').val()) > 0 && $('#new_county option:selected').val() != '') {
            addButton.removeClass('ui-state-disabled').removeAttr('disabled');
        } else {
            //add disabled class if it does not exist
            if (!addButton.hasClass('ui-state-disabled')) {
                addButton.addClass('ui-state-disabled').attr('disabled', 'disabled');
            }
        }
        */
    }

</script>

