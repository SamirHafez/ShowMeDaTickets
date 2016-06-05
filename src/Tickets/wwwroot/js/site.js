angular.module('ticketsApp', ['ui.bootstrap'])
    .controller('TicketViewModel', ['$scope', '$http', function ($scope, $http) {
        $scope.pageSize = 10;

        $scope.stateEnum = {
            idle: 0,
            loadingSuggestion: 1,
            loadingEvents: 2,
            loadingTickets: 3
        };

        $scope.state = $scope.stateEnum.idle;

        $scope.categoryId = null;
        $scope.eventId = null;

        $scope.ticketNumberFilter = null;

        $scope.events = new DataHandler();

        $scope.tickets = new DataHandler();

        function serviceCall(endpoint, params, loadState, success) {
            $scope.state = loadState;
            return $http.get(endpoint, { params: params })
            .then(success)
            .finally(function () { $scope.state = $scope.stateEnum.idle; });
        }

        $scope.getSuggestions = function (query) {
            return serviceCall(
                'api/Search',
                { query: query },
                $scope.stateEnum.loadingSuggestion,
                function (result) { return result.data; })
        };

        $scope.loadEvents = function () {
            return serviceCall(
                'api/Event', {
                    categoryId: $scope.categoryId,
                    page: $scope.events.page
                },
                $scope.stateEnum.loadingEvents,
                function (result) { $scope.events = result.data; })
        };

        $scope.loadTickets = function (numberOfTickets) {
            //Avoid fetching tickets when clicking 'Back to events'
            if (!$scope.eventId)
                return;

            return serviceCall(
                'api/Ticket', {
                    eventId: $scope.eventId,
                    page: $scope.tickets.page,
                    numberOfTickets: numberOfTickets
                },
                $scope.stateEnum.loadingTickets,
                function (result) { $scope.tickets = result.data; })
        };

        $scope.suggestionSelected = function (item) {
            $scope.events = new DataHandler();
            $scope.tickets = new DataHandler();

            $scope.ticketNumberFilter = null;
            $scope.eventId = null;

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