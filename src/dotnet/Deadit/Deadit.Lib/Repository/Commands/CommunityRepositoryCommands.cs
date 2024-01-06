using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deadit.Lib.Repository.Commands;

public sealed class CommunityRepositoryCommands
{
    public const string SelectCommunityByName = @"
        SELECT
            *
        FROM
            View_Community c
        WHERE
            c.community_name = @community_name
        LIMIT
            1;";

    public const string SelectCommunityById = @"
        SELECT
            *
        FROM
            View_Community c
        WHERE
            c.id = @id
        LIMIT
            1;";




    /// <summary>
    /// @name,
    /// @title,
    /// @owner_id,
    /// @description
    /// </summary>
    public const string InsertCommunity = @"
        INSERT INTO
            Community (name, title, owner_id, description)
        VALUES
            (@name, @title, @owner_id, @description);";
}
