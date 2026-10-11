using System;
using System.Collections.Generic;
using System.Text;

namespace ProCredit.BookStore.Application.Requests;

public record CreateAuthorRequest
(
    int AuthorId,
    string Name
);
