# CommunicationHistoryApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**get**](#get) | **GET** /api/CommunicationHistory | |
|[**getById**](#getbyid) | **GET** /api/CommunicationHistory/{id} | |

# **get**
> Array<CommunicationRegistry> get()


### Example

```typescript
import {
    CommunicationHistoryApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CommunicationHistoryApi(configuration);

let pageSize: number; // (optional) (default to undefined)
let page: number; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let from: string; // (optional) (default to undefined)
let to: string; // (optional) (default to undefined)
let sentDateFrom: string; // (optional) (default to undefined)
let sentDateTo: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.get(
    pageSize,
    page,
    title,
    from,
    to,
    sentDateFrom,
    sentDateTo
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **page** | [**number**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **from** | [**string**] |  | (optional) defaults to undefined|
| **to** | [**string**] |  | (optional) defaults to undefined|
| **sentDateFrom** | [**string**] |  | (optional) defaults to undefined|
| **sentDateTo** | [**string**] |  | (optional) defaults to undefined|


### Return type

**Array<CommunicationRegistry>**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getById**
> CommunicationRegistry getById()


### Example

```typescript
import {
    CommunicationHistoryApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CommunicationHistoryApi(configuration);

let id: string; // (default to undefined)

const { status, data } = await apiInstance.getById(
    id
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **id** | [**string**] |  | defaults to undefined|


### Return type

**CommunicationRegistry**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

