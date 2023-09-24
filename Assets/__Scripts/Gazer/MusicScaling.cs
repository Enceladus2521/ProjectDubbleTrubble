using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScaling : MonoBehaviour
{
    private AudioSource _audioSource;
    private Material _material;
    private Shader _shader;


	public float updateStep = 0.1f;
	public int sampleDataLength = 1024;

	private float currentUpdateTime = 0f;

	public float clipLoudness;
	private float[] clipSampleData;
	public float sizeFactor = 1;

	public float minSize = 0;
	public float maxSize = 500;

	// Use this for initialization
	private void Awake()
	{
		clipSampleData = new float[sampleDataLength];
        _audioSource = GetComponent<AudioSource>();
        _material = GetComponent<MeshRenderer>().sharedMaterial;
	}

	// Update is called once per frame
	private void Update()
	{
		currentUpdateTime += Time.deltaTime;
		if (currentUpdateTime >= updateStep)
		{
			currentUpdateTime = 0f;
			_audioSource.clip.GetData(clipSampleData, _audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
			clipLoudness = 0f;
			foreach (var sample in clipSampleData)
			{
				clipLoudness += Mathf.Abs(sample);
			}
			clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for

			clipLoudness *= sizeFactor;
			clipLoudness = Mathf.Clamp(clipLoudness, 0.5f, 1);
            _material.SetFloat("_Amount", clipLoudness);

		}
	}
}
