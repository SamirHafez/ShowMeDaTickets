angular.module('ticketsApp', ['ui.bootstrap'])
    .controller('TicketViewModel', ['$scope', '$http', function ($scope, $http) {
        $scope.pageSize = 10;

        $scope.loading = false;

        $scope.categoryId = null;
        $scope.eventId = null;

        $scope.ticketNumberFilter = null;

        $scope.events = new DataHandler();

        $scope.tickets = new DataHandler();

        function serviceCall(endpoint, params, success) {
            $scope.loading = true;
            return $http.get(endpoint, { params: params })
            .then(success)
            .finally(function () { $scope.loading = false; });
        }

        $scope.getSuggestions = function (query) {
            return serviceCall(
                'api/Search',
                { query: query },
                function (result) { return result.data; })
        };

        $scope.loadEvents = function () {
            return serviceCall(
                'api/Event', {
                    categoryId: $scope.categoryId,
                    page: $scope.events.page
                },
                function (result) { $scope.events = result.data; })
        };

        $scope.loadTickets = function (numberOfTickets) {
            //Avoid fetching tickets when clicking 'Back to events'
            if ($scope.tickets.totalItems === 0 && $scope.tickets.items.length)
                return;

            return serviceCall(
                'api/Ticket', {
                    eventId: $scope.eventId,
                    page: $scope.tickets.page,
                    numberOfTickets: numberOfTickets
                },
                function (result) { $scope.tickets = result.data; })
        };

        $scope.suggestionSelected = function (item) {
            $scope.events = new DataHandler();
            $scope.tickets = new DataHandler();

            $scope.ticketNumberFilter = null;

            $scope.categoryId = item.categoryId;
            $scope.loadEvents();
        };

        $scope.eventSelected = function (event) {
            $scope.tickets = new DataHandler();

            $scope.ticketNumberFilter = null;

            $scope.eventId = event.id;
            $scope.loadTickets();
        };
    }]);

function DataHandler() {
    return {
        totalItems: 0,
        page: 1,
        items: []
    };
}