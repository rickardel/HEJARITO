$(document).ready(function () {
    $('#activitiestable').DataTable(
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
                }
        });

    $('#coursestable').DataTable(
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
                }
        });

    $('#contactstable').DataTable(
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
                }
        });
});