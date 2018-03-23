$(document).ready(function () {
    $('#activitiestable-small').DataTable(
        {
            language:
                {
                    lengthMenu: "Visa _MENU_ rader per sida",
                    search: 'Sök:',
                    info: "",
                    infoFiltered: "(filtrerat från _MAX_ rader)",
                    paginate: {
                        previous: 'Föregående',
                        next: 'Nästa',
                    }
                },
            searching: false,
            paging: false,
            lengthMenu: [[3, 10, 25, 50, -1], [3, 10, 25, 50, "All"]]
        });

    $('#coursestable-small').DataTable(
        {
            language:
                {
                    lengthMenu: "Visa _MENU_ rader per sida",
                    search: 'Sök:',
                    info: "",
                    infoFiltered: "(filtrerat från _MAX_ rader)",
                    paginate: {
                        previous: 'Föregående',
                        next: 'Nästa'
                    }
                },
            searching: false,
            paging: false,
            lengthMenu: [[3, 10, 25, 50, -1], [3, 10, 25, 50, "All"]]
        });

    $('#contactstable-small').DataTable(
        {
            language:
                {
                    lengthMenu: "Visa _MENU_ rader per sida",
                    search: 'Sök:',
                    info: "",
                    infoFiltered: "(filtrerat från _MAX_ rader)",
                    paginate: {
                        previous: 'Föregående',
                        next: 'Nästa'
                    }
                },
            searching: false,
            paging: false,
            lengthMenu: [[3, 10, 25, 50, -1], [3, 10, 25, 50, "All"]]
        });
});

$(document).ready(function () {
    $('#activitiestable-large').DataTable(
        {
            language:
                {
                    lengthMenu: "Visa _MENU_ rader per sida",
                    search: 'Sök:',
                    info: "Visar _START_ till _END_ av _TOTAL_ rader",
                    infoFiltered: "(filtrerat från _MAX_ rader)",
                    paginate: {
                        previous: 'Föregående',
                        next: 'Nästa',
                    }
                },
            lengthMenu: [[3, 10, 25, 50, -1], [3, 10, 25, 50, "All"]]
        });

    $('#coursestable-large').DataTable(
        {
            language:
                {
                    lengthMenu: "Visa _MENU_ rader per sida",
                    search: 'Sök:',
                    info: "Visar _START_ till _END_ av _TOTAL_ rader",
                    infoFiltered: "(filtrerat från _MAX_ rader)",
                    paginate: {
                        previous: 'Föregående',
                        next: 'Nästa'
                    }
                },
            lengthMenu: [[3, 10, 25, 50, -1], [3, 10, 25, 50, "All"]]
        });

    $('#contactstable-large').DataTable(
        {
            language:
                {
                    lengthMenu: "Visa _MENU_ rader per sida",
                    search: 'Sök:',
                    info: "Visar _START_ till _END_ av _TOTAL_ rader",
                    infoFiltered: "(filtrerat från _MAX_ rader)",
                    paginate: {
                        previous: 'Föregående',
                        next: 'Nästa'
                    }
                },
            lengthMenu: [[3, 10, 25, 50, -1], [3, 10, 25, 50, "All"]]
        });
});