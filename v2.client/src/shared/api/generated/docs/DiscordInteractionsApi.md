# DiscordInteractionsApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createDiscordInteractions**](#creatediscordinteractions) | **POST** /api/discord/interactions | |

# **createDiscordInteractions**
> createDiscordInteractions()


### Example

```typescript
import {
    DiscordInteractionsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DiscordInteractionsApi(configuration);

const { status, data } = await apiInstance.createDiscordInteractions();
```

### Parameters
This endpoint does not have any parameters.


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

