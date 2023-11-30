import http from "k6/http";
import { check, sleep } from "k6";
import { group } from "k6";

export const options = {
  scenarios: {
    first_scenario: {
      executor: "constant-vus",
      vus: 10,
      duration: "30s",
    },
    second_scenario: {
      executor: "constant-vus",
      vus: 20,
      duration: "20s",
      startTime: "30s",
    },
    third_scenario: {
      executor: "constant-vus",
      vus: 10,
      duration: "20s",
      startTime: "50s",
    },
  },
};

export default function () {
  const random_uuid = Math.floor(Math.random() * 999999);

  //Login
  const postTokenUrl = "https://localhost:7112/Auth/LogIn";
  const payloadToken = JSON.stringify({
    email: "Admin@123",
    password: "Admin@123",
  });

  const postParams = {
    headers: {
      "Content-Type": "application/json",
    },
  };
  let tokenResponse;
  group("GetToken", function () {
    tokenResponse = http.post(postTokenUrl, payloadToken, postParams, {
      tags: { name: "GetToken" },
    });
  });
  const token = JSON.parse(tokenResponse.body).token;
  check(tokenResponse, { "LogIn status was 200": (r) => r.status == 200 });

  const authParams = {
    headers: {
      Authorization: `Bearer ${token}`,
      "Content-Type": "application/json",
    },
  };

  //Get All Accounts
  const getAccountsUrl = "https://localhost:7112/Accounts";
  let accountsResp;
  group("GetAllAccounts", function () {
    accountsResp = http.get(getAccountsUrl, authParams, {
      tags: { name: "GetAllAccounts" },
    });
  });

  const accounts = JSON.parse(accountsResp.body);
  const adminAccountId = accounts.find(
    (account) => account.firstName === "Admin"
  ).id;
  check(accountsResp, { "Accounts status was 200": (r) => r.status == 200 });

  //Get AccountById
  let accountsByIdResp;
  group("GetAccountById", function () {
    accountsByIdResp = http.get(
      getAccountsUrl + `/${adminAccountId}`,
      authParams,
      {
        tags: { name: "GetAccountById" },
      }
    );
    for (let i = 0; i < 20; i++) {
      http.get(getAccountsUrl + `/${adminAccountId}`, authParams, {
        tags: { name: "GetAccountById" },
      });
    }
  });

  check(accountsByIdResp, {
    "Accounts status was 200": (r) => r.status == 200,
  });

  //Create ArticleTheme
  const themeUrl = "https://localhost:7112/ArticleTheme";
  const payloadTheme = JSON.stringify({
    name: `TestTheme ${random_uuid}`,
  });

  let postThemeResponse;
  group("CreateArticleTheme", function () {
    postThemeResponse = http.post(themeUrl, payloadTheme, authParams, {
      tags: { name: "CreateArticleTheme" },
    });
  });

  const themeId = JSON.parse(postThemeResponse.body).id;
  check(postThemeResponse, {
    "Create New Theme status was 200": (r) => r.status == 200,
  });

  //Get ArticleTheme By Id
  let themeById;
  group("GetArticleTheme", function () {
    themeById = http.get(themeUrl + `/${themeId}`, authParams, {
      tags: { name: "GetArticleTheme", group: "GetArticleTheme" },
    });
    for (let i = 0; i < 20; i++) {
      http.get(themeUrl + `/${themeId}`, authParams, {
        tags: { name: "GetArticleTheme", group: "GetArticleTheme" },
      });
    }
  });
  check(themeById, {
    "Get ArticleTheme By ID status was 200": (r) => r.status == 200,
  });

  //Get All ArticleThemes
  let allTheme;
  group("GetAllArticleThemes", function () {
    allTheme = http.get(themeUrl, authParams, {
      tags: { name: "GetAllArticleThemes" },
    });
  });

  check(allTheme, {
    "Get All ArticleThemes status was 200": (r) => r.status == 200,
  });

  //Create Article
  const articleUrl = "https://localhost:7112/Article";
  const payloadArticle = JSON.stringify({
    title: `Test Asrticle ${random_uuid}`,
    description: "Test Article",
    text: `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Vitae ultricies leo integer malesuada. Pellentesque adipiscing commodo elit at imperdiet dui accumsan. Facilisis gravida neque convallis a cras semper auctor. In tellus integer feugiat scelerisque varius morbi enim nunc faucibus.`,
    accountId: adminAccountId,
  });

  let postArticleResponse;
  group("CreateArticle", function () {
    postArticleResponse = http.post(articleUrl, payloadArticle, authParams, {
      tags: { name: "CreateArticle" },
    });
  });

  const article = JSON.parse(postArticleResponse.body);
  check(postArticleResponse, {
    "Create New Article status was 200": (r) => r.status == 200,
  });

  //Get Article By Id
  let articleById;
  group("GetArticleById", function () {
    articleById = http.get(articleUrl + `/${article.id}`, authParams, {
      tags: { name: "GetArticleById" },
    });
    for (let i = 0; i < 20; i++) {
      http.get(articleUrl + `/${article.id}`, authParams, {
        tags: { name: "GetArticleById" },
      });
    }
  });

  check(articleById, {
    "Get Article By ID status was 200": (r) => r.status == 200,
  });

  //Patch Article
  const patchPayloadArticle = JSON.stringify([themeId]);

  let patchArticleResponse;
  group("PatchArticle", function () {
    patchArticleResponse = http.patch(
      articleUrl + `/${article.id}`,
      patchPayloadArticle,
      authParams,
      {
        tags: { name: "PatchArticle" },
      }
    );
  });

  check(patchArticleResponse, {
    "Patch New Article status was 200": (r) => r.status == 200,
  });

  //Get ArticleTheme By article Id
  let articleThemeByArticleId;
  group("GetArticleThemeByArticle", function () {
    articleThemeByArticleId = http.get(
      themeUrl + `/article/${article.id}`,
      authParams,
      {
        tags: {
          name: "GetArticleThemeByArticle",
        },
      }
    );
  });

  check(articleThemeByArticleId, {
    "Get ArticleTheme By article Id status was 200": (r) => r.status == 200,
  });

  //Get Article By theme Id
  let articleByThemeId;
  group("GetArticleByTheme", function () {
    articleByThemeId = http.get(articleUrl + `/theme/${themeId}`, authParams, {
      tags: { name: "GetArticleByTheme" },
    });
  });

  check(articleByThemeId, {
    "Get Article By theme Id status was 200": (r) => r.status == 200,
  });

  //Get Article By account Id
  let articleByAccountId;
  group("GetArticleByAccount", function () {
    articleByAccountId = http.get(
      articleUrl + `/accountid/${adminAccountId}`,
      authParams,
      {
        tags: { name: "GetArticleByAccount" },
      }
    );
  });

  check(articleByAccountId, {
    "Get Article By account Id status was 200": (r) => r.status == 200,
  });

  //Create Comment
  const commentUrl = "https://localhost:7112/Comment";
  const payloadComment = JSON.stringify({
    accountId: adminAccountId,
    articleId: article.id,
    text: `Some comment with ${random_uuid}`,
  });

  let postCommentResponse;
  group("CreateComment", function () {
    postCommentResponse = http.post(commentUrl, payloadComment, authParams, {
      tags: { name: "CreateComment" },
    });
  });

  const comment = JSON.parse(postCommentResponse.body);
  check(postCommentResponse, {
    "Create New Comment status was 200": (r) => r.status == 200,
  });

  //Get Comment By Id
  let commentById;
  group("GetComment", function () {
    commentById = http.get(commentUrl + `/${comment.id}`, authParams, {
      tags: { name: "GetComment" },
    });
  });

  check(commentById, {
    "Get Comment By Id status was 200": (r) => r.status == 200,
  });

  //Change Comment
  const payloadChangeComment = JSON.stringify({
    id: comment.id,
    accountId: adminAccountId,
    articleId: article.id,
    text: `Some changed comment with ${random_uuid}`,
  });

  let postChangeCommentResponse;
  group("ChangeComment", function () {
    postChangeCommentResponse = http.put(
      commentUrl,
      payloadChangeComment,
      authParams,
      {
        tags: { name: "ChangeComment" },
      }
    );
  });

  check(postChangeCommentResponse, {
    "Change New Comment status was 200": (r) => r.status == 200,
  });

  //Get Comment By Article Id

  let commentByArticleId;
  group("GetCommentByArticleId", function () {
    commentByArticleId = http.get(
      commentUrl + `/article/${article.id}`,
      authParams,
      {
        tags: { name: "GetCommentByArticleId" },
      }
    );
  });

  check(commentByArticleId, {
    "Get Comment By Article Id status was 200": (r) => r.status == 200,
  });

  //Delete Comment
  let deleteComment;
  group("DeleteComment", function () {
    deleteComment = http.del(commentUrl + `/${comment.id}`, null, authParams, {
      tags: { name: "DeleteComment" },
    });
  });

  check(deleteComment, {
    "Delete Comment status was 200": (r) => r.status == 200,
  });

  //Delete Article
  let deleteArticle;
  group("DeleteArticle", function () {
    deleteArticle = http.del(articleUrl + `/${article.id}`, null, authParams, {
      tags: { name: "DeleteArticle" },
    });
  });

  check(deleteArticle, {
    "Delete Article status was 200": (r) => r.status == 200,
  });

  //Delete ArticleTheme
  let deleteTheme;
  group("DeleteArticleTheme", function () {
    deleteTheme = http.del(themeUrl + `/${themeId}`, null, authParams, {
      tags: { name: "DeleteArticleTheme" },
    });
  });

  check(deleteTheme, {
    "Delete ArticleTheme status was 200": (r) => r.status == 200,
  });
}
