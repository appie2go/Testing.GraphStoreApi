# Testing.GraphStoreApi

This is an API that allows you to story any graph you like. This project is built for testing/demo purposes only.

This api has an infinate number of endpoints. The endpoint structure is as follows.

## Where do I get it?
To run this API, pull the following image from the Docker hub and run it:

```powershell
docker pull albertstarreveld/testing.graphstoreapi

docker run -d -p 8123:80 --name graphstoreapi albertstarreveld/testing.graphstoreapi
```

## Storing objects
Like you would expect in any API, creating a new object is done by posting to the API:

```powershell
curl -X 'POST' \
  'https://localhost:8123/{whatever}' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 42,
  "anything": "foo",
  "you": {
    "like": "In any structure you want"
  }
}'

```

You may post to anything endpoint you like. The endpoint is a variable. So, you may post to:
* https://hostname:8443/apples
* https://hostname:8443/foobar
* https://hostname:8443/johhny
* https://hostname:8443/c257551d-b924-41dc-9a22-15686a7795a9
* And so forth, anything will work

If you do not provide an Id in the request body, the API will create one for you. It will be of type GUID/UUID.

If you create an object with an id that already exists, the API will respond with statuscode 409: Conflict. Your request will not be processed in that case.

## Listing objects

The objects you've posted to the endpoint of your choice are stored in memory. If you execute a get request to the endpoint, you'll see the objects you've created:

```powershell
curl -X 'GET' \
  'https://localhost:8123/{whatever}' \
  -H 'accept: application/json'
```

Will in this case respond with: 
```json
[{
    "id": 42,
    "anything": "foo",
    "you": {
        "like": "In any structure you want"
    }
},{
    "id": 43,
    "anything": "bar",
    "you": {
        "like": "In any structure you want"
    }
}]
```

## Fetching a single object

You may also get a single object. Do so by executing the following request

```powershell
curl -X 'GET' \
  'https://localhost:8123/{whatever}/42' \
  -H 'accept: application/json'
```

Will in this case respond with: 
```json
{
    "id": 42,
    "anything": "foo",
    "you": {
        "like": "In any structure you want"
    }
}
```

If you request an object that does not request, obviously, you'll get a 404.

## Updating objects
Like you would expect, updating an object is done by executing a PUT request:

```powershell
curl -X 'PUT' \
  'https://localhost:8123/{whatever}/42' \
  -H 'accept: */*' \
  -H 'Content-Type: application/json' \
  -d '{
  "id": 42,
  "anything": "bar!!",
  "you": {
    "like": "In any structure you want"
  }
}'
```

## Deleting objects

Deleting objects is done by executing a DELETE http request:


```powershell
curl -X 'DELETE' \
  'https://localhost:8123/test/42' \
  -H 'accept: */*'
```

## Disclaimer
This API is for testing purposes only.