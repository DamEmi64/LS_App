# EventInvoicesApi

All URIs are relative to *http://localhost*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**create**](#create) | **POST** /api/EventInvoices | |
|[**get**](#get) | **GET** /api/EventInvoices | |

# **create**
> create()


### Example

```typescript
import {
    EventInvoicesApi,
    Configuration,
    CreateEventInvoiceDto
} from './api';

const configuration = new Configuration();
const apiInstance = new EventInvoicesApi(configuration);

let createEventInvoiceDto: CreateEventInvoiceDto; // (optional)

const { status, data } = await apiInstance.create(
    createEventInvoiceDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **createEventInvoiceDto** | **CreateEventInvoiceDto**|  | |


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json-patch+json, application/json, text/json, application/*+json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **get**
> get()


### Example

```typescript
import {
    EventInvoicesApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new EventInvoicesApi(configuration);

const { status, data } = await apiInstance.get();
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

