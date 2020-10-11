const GRAPHQL_ENDPOINT = "/graphql";

function query(
  query: string,
  variables: Record<string, any>
): Promise<Record<string, any>> {
  console.log({ query, variables });
  return fetch(GRAPHQL_ENDPOINT, {
    method: "POST",
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ query, variables }),
  })
    .then((r: Response) => r.json())
    .then((r: any) => {
      console.log(r);
      return r.data;
    });
}

export default { query };
