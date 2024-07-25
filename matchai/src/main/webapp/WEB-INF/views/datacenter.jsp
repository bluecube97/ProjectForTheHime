<%@ page language="java" contentType="text/html; charset=UTF-8" pageEncoding="UTF-8" %>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html>
<head>
    <meta charset="UTF-8">
    <title>자유 게시판</title>
    <link rel="stylesheet" type="text/css" href="/resource/css/datacenter.css">
    <script src="/resource/js/datacenter.js" defer></script>
</head>
<body>
    <div class="sort">
        <label class="sortType">
            <input type="radio" name="order" value="asc" onchange="submitSortForm()">
            Ascending
            <input type="radio" name="order" value="desc" onchange="submitSortForm()">
            Descending
        </label>
        <form id="sortForm" action="/datacenter/sort" method="get">
            <label class="sortBy">
                <select name="sortBy" onchange="submitSortForm()">
                    <c:forEach var="sort" items="${sortList}">
                        <option value="${sort}">${sort}</option>
                    </c:forEach>
                </select>
            </label>
        </form>
    </div>
    <div class="column">
        <div class="kbobat">
            <div class="year">year</div>
            <div class="teamcode">teamcode</div>
            <div class="ab">ab</div>
            <div class="r">r</div>
            <div class="twob">twob</div>
            <div class="threeb">threeb</div>
            <div class="hr">hr</div>
            <div class="rbi">rbi</div>
            <div class="sb">sb</div>
            <div class="cs">cs</div>
            <div class="bb">bb</div>
            <div class="so">so</div>
            <div class="avg">avg</div>
            <div class="obp">obp</div>
            <div class="slg">slg</div>
            <div class="league_code">league_code</div>
            <div class="teamname_kr">teamname_kr</div>
        </div>
        <div class="kbobat-content">
            <c:forEach var="kbobat" items="${kboBatList}">
                <div class="kbobat-row">
                    <div class="year">${kbobat.year}</div>
                    <div class="teamcode">${kbobat.teamcode}</div>
                    <div class="ab">${kbobat.ab}</div>
                    <div class="r">${kbobat.r}</div>
                    <div class="twob">${kbobat.twob}</div>
                    <div class="threeb">${kbobat.threeb}</div>
                    <div class="hr">${kbobat.hr}</div>
                    <div class="rbi">${kbobat.rbi}</div>
                    <div class="sb">${kbobat.sb}</div>
                    <div class="cs">${kbobat.cs}</div>
                    <div class="bb">${kbobat.bb}</div>
                    <div class="so">${kbobat.so}</div>
                    <div class="avg">${kbobat.avg}</div>
                    <div class="obp">${kbobat.obp}</div>
                    <div class="slg">${kbobat.slg}</div>
                    <div class="league_code">${kbobat.league_code}</div>
                    <div class="teamname_kr">${kbobat.teamname_kr}</div>
                </div>
            </c:forEach>
        </div>
        <div class="kbopit">
            <div class="year">year</div>
            <div class="teamcode">teamcode</div>
            <div class="gs">gs</div>
            <div class="cg">cg</div>
            <div class="sho">sho</div>
            <div class="s">s</div>
            <div class="ip">ip</div>
            <div class="er">er</div>
            <div class="r">r</div>
            <div class="hr">hr</div>
            <div class="bb">bb</div>
            <div class="hp">hp</div>
            <div class="so">so</div>
            <div class="league_code">league_code</div>
            <div class="teamname_kr">teamname_kr</div>
        </div>
        <div class="kbopit-content">
            <c:forEach var="kbopit" items="${kboPitList}">
                <div class="kbopit-row">
                    <div class="year">${kbopit.year}</div>
                    <div class="teamcode">${kbopit.teamcode}</div>
                    <div class="gs">${kbopit.gs}</div>
                    <div class="cg">${kbopit.cg}</div>
                    <div class="sho">${kbopit.sho}</div>
                    <div class="s">${kbopit.s}</div>
                    <div class="ip">${kbopit.ip}</div>
                    <div class="er">${kbopit.er}</div>
                    <div class="r">${kbopit.r}</div>
                    <div class="hr">${kbopit.hr}</div>
                    <div class="bb">${kbopit.bb}</div>
                    <div class="hp">${kbopit.hp}</div>
                    <div class="so">${kbopit.so}</div>
                    <div class="league_code">${kbopit.league_code}</div>
                    <div class="teamname_kr">${kbopit.teamname_kr}</div>
                </div>
            </c:forEach>
        </div>
        <div class="mlbbat">
            <div class="year">year</div>
            <div class="teamcode">teamcode</div>
            <div class="ab">ab</div>
            <div class="r">r</div>
            <div class="twob">twob</div>
            <div class="threeb">threeb</div>
            <div class="hr">hr</div>
            <div class="rbi">rbi</div>
            <div class="sb">sb</div>
            <div class="cs">cs</div>
            <div class="bb">bb</div>
            <div class="so">so</div>
            <div class="avg">avg</div>
            <div class="obp">obp</div>
            <div class="slg">slg</div>
            <div class="league_code">league_code</div>
            <div class="teamname_kr">teamname_kr</div>
        </div>
        <div class="mlbbat-content">
            <c:forEach var="mlbbat" items="${mlbBatList}">
                <div class="mlbbat-row">
                    <div class="year">${mlbbat.year}</div>
                    <div class="teamcode">${mlbbat.teamcode}</div>
                    <div class="ab">${mlbbat.ab}</div>
                    <div class="r">${mlbbat.r}</div>
                    <div class="twob">${mlbbat.twob}</div>
                    <div class="threeb">${mlbbat.threeb}</div>
                    <div class="hr">${mlbbat.hr}</div>
                    <div class="rbi">${mlbbat.rbi}</div>
                    <div class="sb">${mlbbat.sb}</div>
                    <div class="cs">${mlbbat.cs}</div>
                    <div class="bb">${mlbbat.bb}</div>
                    <div class="so">${mlbbat.so}</div>
                    <div class="avg">${mlbbat.avg}</div>
                    <div class="obp">${mlbbat.obp}</div>
                    <div class="slg">${mlbbat.slg}</div>
                    <div class="league_code">${mlbbat.league_code}</div>
                    <div class="teamname_kr">${mlbbat.teamname_kr}</div>
                </div>
            </c:forEach>
        </div>
        <div class="mlbpit">
            <div class="year">year</div>
            <div class="teamcode">teamcode</div>
            <div class="gs">gs</div>
            <div class="cg">cg</div>
            <div class="sho">sho</div>
            <div class="s">s</div>
            <div class="ip">ip</div>
            <div class="er">er</div>
            <div class="r">r</div>
            <div class="hr">hr</div>
            <div class="bb">bb</div>
            <div class="hp">hp</div>
            <div class="so">so</div>
            <div class="league_code">league_code</div>
            <div class="teamname_kr">teamname_kr</div>
        </div>
        <div class="mlbpit-content">
            <c:forEach var="mlbpit" items="${mlbPitList}">
                <div class="mlbpit-row">
                    <div class="year">${mlbpit.year}</div>
                    <div class="teamcode">${mlbpit.teamcode}</div>
                    <div class="gs">${mlbpit.gs}</div>
                    <div class="cg">${mlbpit.cg}</div>
                    <div class="sho">${mlbpit.sho}</div>
                    <div class="s">${mlbpit.s}</div>
                    <div class="ip">${mlbpit.ip}</div>
                    <div class="er">${mlbpit.er}</div>
                    <div class="r">${mlbpit.r}</div>
                    <div class="hr">${mlbpit.hr}</div>
                    <div class="bb">${mlbpit.bb}</div>
                    <div class="hp">${mlbpit.hp}</div>
                    <div class="so">${mlbpit.so}</div>
                    <div class="league_code">${mlbpit.league_code}</div>
                    <div class="teamname_kr">${mlbpit.teamname_kr}</div>
                </div>
            </c:forEach>
        </div>
    </div>
</body>
</html>
