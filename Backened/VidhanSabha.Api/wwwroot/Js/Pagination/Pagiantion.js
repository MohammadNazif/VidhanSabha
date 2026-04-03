function initPaginatedTable(config) {
    const {
        apiUrl,
        tableSelector,
        pageSizeSelector,
        queryParamsFunction = () => ({}),
        columns = []
    } = config;

    let dataTable;

    function initDataTable() {
        // Destroy existing instance before init
        if ($.fn.DataTable.isDataTable(tableSelector)) {
            $(tableSelector).DataTable().destroy();
            $(tableSelector).empty(); // clear table body
        }

        dataTable = $(tableSelector).DataTable({
            serverSide: true,
            processing: true,
            searching: true,
            paging: true,
            autoWidth: false,
            responsive: false,
            lengthChange: true,
            pageLength: parseInt($(pageSizeSelector).val()) || 50,
            ajax: function (data, callback) {
                const pageNumber = Math.floor(data.start / data.length) + 1;
                const items = data.length;
                const extraParams = queryParamsFunction() || {};

                const params = {
                    PageNumber: pageNumber,
                    Items: items,
                    search: data.search.value,
                    ...extraParams
                };

                $.ajax({
                    url: apiUrl,
                    method: 'GET',
                    data: params,
                    xhrFields: {
                        withCredentials: true // send cookies
                    },
                   
                }).done(res => {  
                    if (res.success) {
                        callback({
                            data: res.data.data,
                            recordsTotal: res.data.totalRecords,
                            recordsFiltered: res.data.totalRecords
                        });
                    
                    } else {
                        console.log("some error occured");
                            //Swal.fire('Error', res.message || 'Data Not Found', 'error');
                            callback({
                                data: [],
                                recordsTotal: 0,
                                recordsFiltered: 0
                            });
                        }
                    })
                    .fail((res) => {
                        console.log(res);
                        //Swal.fire('Error', res.message || 'Data Not Found ', 'error');
                        callback({
                            data: [],
                            recordsTotal: 0,
                            recordsFiltered: 0
                        });
                    });
            },
            columns: columns,
            className: 'text-nowrap',
            order: [],
            language: {
                emptyTable: "No data available"
            },

            drawCallback: function () {
                this.api().columns.adjust();
            }
        });
    }

    // Initial load
    initDataTable();

    // Re-init on page size change
    $(pageSizeSelector).on('change', function () {
        initDataTable();
    });

    return {
        reload: function () {
            if (dataTable) {
                dataTable.ajax.reload(null, false); // false = keep current page
            }
        }
    };
}
