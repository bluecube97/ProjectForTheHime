<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>실제 경기기록</title>
    <link rel="stylesheet" type="text/css" href="/resource/css/mainboard.css">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet"
          integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
    <script src="https://cdn.jsdelivr.net/npm/chart.js@3.7.0/dist/chart.min.js" defer></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"
            crossorigin="anonymous"></script>
    <script src="/resource/js/mainboard.js" defer></script>
</head>
<body>
<header>
    <div class="logo">
        <button type="button" class="btn-logo" id="btn-logo">
            <img src="/resource/images/mainlogo/MATCHAIBoard.png" alt="로고" class="logo-img">
        </button>
    </div>
    <nav>
        <div class="main_menu">
            <ul>
                <li><a href="/board/aibaseball" class="menu-btn">AI 경기예상</a></li>
                <li><a href="/board/actbaseball" class="menu-btn">실제 경기기록</a></li>
                <!-- Other menu items -->
            </ul>
        </div>
    </nav>
</header>
<div class="content">
    <main>
        <div class="today-games">
            <div class="games-container" id="games-container">
                <div id="mlb-content">
                    <c:forEach var="game" items="${gamelist}">
                        <div class="games-row">
                            <form action="/board/gamedetail" method="get">
                                <input type="hidden" name="team1" value="${game.team1}">
                                <input type="hidden" name="team2" value="${game.team2}">
                                <input type="hidden" name="matchcode" value="${game.matchcode}">
                                <button type="button" class="summary" id="summarybtn1"
                                        onclick="window.location.href='/board/actdetail?matchcode=${game.matchcode}&team1=${game.team1}&team2=${game.team2}'"
                                        style="cursor: pointer;">${game.brd_date} ${game.title}
                                </button>
                            </form>
                        </div>
                    </c:forEach>

                    <!-- Pagination for actual games -->
                    <div class="pagination">
                        <c:choose>
                            <c:when test="${currentPage > 1}">
                                <a href="?page=${currentPage - 1}" class="btn btn-primary">Previous</a>
                            </c:when>
                        </c:choose>
                        <c:forEach begin="1" end="${totalRecords / pageSize + 1}" var="i">
                            <c:choose>
                                <c:when test="${i == currentPage}">
                                    <span class="btn btn-secondary">${i}</span>
                                </c:when>
                                <c:otherwise>
                                    <a href="?page=${i}" class="btn btn-primary">${i}</a>
                                </c:otherwise>
                            </c:choose>
                        </c:forEach>
                        <c:choose>
                            <c:when test="${currentPage * pageSize < totalRecords}">
                                <a href="?page=${currentPage + 1}" class="btn btn-primary">Next</a>
                            </c:when>
                        </c:choose>
                    </div>
                </div>
            </div>
        </div>
    </main>
</div>

<footer class="footer">
    <!-- Footer content here -->
</footer>
</body>
</html>
