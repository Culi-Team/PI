using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIFilmAutoDetachCleanMC.Recipe
{
    public class MotionConfigList
    {
        public MotionConfigList([FromKeyedServices("MotionAjinConfig")] MotionAjinConfig motionAjinConfig,
                              [FromKeyedServices("MotionInovanceConfig")] MotionInovanceConfig motionInovanceConfig,
                              [FromKeyedServices("VinylCleanEncoderConfig")] VinylCleanEncoderConfig vinylCleanEncoderConfig)
        {
            MotionAjinConfig = motionAjinConfig;
            MotionInovanceConfig = motionInovanceConfig;
            VinylCleanEncoderConfig = vinylCleanEncoderConfig;
            MotionAjinConfig.Name = "Motion Ajin";
            MotionInovanceConfig.Name = "Motion Inovance";
            VinylCleanEncoderConfig.Name = "Vinyl Clean Encoder";
        }

        public MotionAjinConfig MotionAjinConfig { get; }
        public MotionInovanceConfig MotionInovanceConfig { get; }
        public VinylCleanEncoderConfig VinylCleanEncoderConfig { get; }
    }
}
