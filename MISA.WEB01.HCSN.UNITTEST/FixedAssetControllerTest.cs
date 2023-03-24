using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using MISA.WEB01.HCSN.Common.entities;
using MISA.WEB01.HCSN.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.WEB01.HCSN.UNITTEST
{
    public class FixedAssetControllerTest
    {
        [Test]
        public void InsertFixedAsset_FixedAsset_ReturnSucces()
        {
            //Arrange
             var fixedAsset= new FixedAsset(){
             fixed_asset_id = new Guid("833c15c3-7d84-40d5-8ca8-6f903b0eb716"),
             fixed_asset_code = "TS98313",
             fixed_asset_name = "iphone 12",
                 department_id = new Guid("11452b0c-768e-5ff7-0d63-eeb1d8ed8cef"),
                 department_code = "D003",
                 department_name = "Phòng Hội trường lớn",
                 fixed_asset_category_id = new Guid("45ac3d26-18f2-18a9-3031-644313fbb055"),
                 fixed_asset_category_code = "AFT006",
                 fixed_asset_category_name = "Máy tính lượng tử",
                 //purchase_date = new Timestamp("2013-03-15T00:00:00+07:00"),
                 cost = 10000000,
                 quantity = 3,
                 depreciation_rate = 10,
                 depreciation_value = 1000000,
                 //tracked_year = "2013-03-15T00:00:00+07:00",
                 life_time = 10,
                 production_year = 2023,

                 //created_date = "2023-03-20T22:14:42+07:00",
                 //modified_date = "2023-03-20T22:14:42+07:00"

             };

            var expectedResult = new ObjectResult(
               new Guid("833c15c3-7d84-40d5-8ca8-6f903b0eb716")
            ); 
            expectedResult.StatusCode = 201;
            var fakefixedAssetDL = new FakeFixedAssetDL();
            var fixedAssetController =new FixedAssetsController(fakefixedAssetDL);

            //Act
            var actualResult = (ObjectResult)fixedAssetController.InsertRecord(fixedAsset);
        
            //Assert
            Assert.AreEqual(expectedResult.StatusCode, actualResult.StatusCode);
        }

       
    }
}
