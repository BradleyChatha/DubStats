const GRAPHQL_ENDPOINT = "/graphql";

function query(query: string, variables: object): Promise<Object> {
  return fetch(GRAPHQL_ENDPOINT, {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ query, variables }),
  }).then((r: Response) => r.json());
}

export default { query };
