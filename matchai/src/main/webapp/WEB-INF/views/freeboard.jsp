<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>자유 게시판</title>
    <link rel="stylesheet" type="text/css" href="/resource/css/freeboard.css">
    <script src="/resource/js/freeboard.js" defer></script>
</head>
<body>
    <div class="column">
        <div class="table-row">
            <div class="title">제목</div>
            <div class="username">글쓴이</div>
            <div class="date">날짜</div>
            <div class="hitcount">추천수</div>
        </div>
        <div class="table-content">
            <c:forEach var="board" items="${freeboardlist}">
                <div class="table-row">
                    <div class="title">
                        <a href="/freeboard/detail/${board.id}">${board.title}</a>
                    </div>
                    <div class="username">${board.username}</div>
                    <div class="date">${board.date}</div>
                    <div class="hitcount">${board.hitcount}</div>
                </div>
            </c:forEach>
        </div>
    </div>
</body>
</html>
